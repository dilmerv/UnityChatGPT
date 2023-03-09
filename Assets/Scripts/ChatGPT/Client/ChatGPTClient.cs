using DilmerGames.Core.Singletons;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTClient : Singleton<ChatGPTClient>
{
    [SerializeField]
    private ChatGTPSettings chatGTPSettings;

    public IEnumerator Ask(string prompt, Action<ChatGPTResponse> callBack)
    {
        var url = chatGTPSettings.debug ? $"{chatGTPSettings.apiURL}?debug=true" : chatGTPSettings.apiURL;

        using(UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            var requestParams = JsonConvert.SerializeObject(new ChatGPTRequest
            {
                Model = chatGTPSettings.apiModel,
                Messages = new ChatGPTChatMessage[]
                   {
                       new ChatGPTChatMessage
                       {
                            Role = "user",
                            Content = prompt
                       }
                   }
            });

            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestParams);
            
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.disposeDownloadHandlerOnDispose = true;
            request.disposeUploadHandlerOnDispose = true;
            request.disposeCertificateHandlerOnDispose = true;

            request.SetRequestHeader("Content-Type", "application/json");

            // required to authenticate against OpenAI
            request.SetRequestHeader("Authorization", $"Bearer {chatGTPSettings.apiKey}");
            request.SetRequestHeader("OpenAI-Organization", chatGTPSettings.apiOrganization);

            var requestStartDateTime = DateTime.Now;

            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string responseInfo = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<ChatGPTResponse>(responseInfo)
                    .CodeCleanUp();

                response.ResponseTotalTime = (DateTime.Now - requestStartDateTime).TotalMilliseconds;

                callBack(response);
            }
        }
    }
}
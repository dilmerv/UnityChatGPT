using DilmerGames.Core.Singletons;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTClient : Singleton<ChatGPTClient>
{
    [SerializeField]
    private ChatGPTSettings chatGPTSettings;

    public IEnumerator Ask(string prompt, System.Action<ChatGPTResponse> callBack)
    {
        var url = chatGPTSettings.debug ? $"{chatGPTSettings.apiURL}?debug=true" : chatGPTSettings.apiURL;

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(
                JsonConvert.SerializeObject(new ChatGPTRequest
                {
                    Question = chatGPTSettings.reminders.Length == 0 ? $"{prompt}"
                        : $"{prompt}, {string.Join(", ", chatGPTSettings.reminders)}"
                }));

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.disposeUploadHandlerOnDispose = true;
            request.disposeCertificateHandlerOnDispose = true;
            request.disposeDownloadHandlerOnDispose = true;

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                var response = (new ChatGPTResponse { Data = responseText }).CodeCleanUp();
                callBack(response);
            }
        }
    }
}
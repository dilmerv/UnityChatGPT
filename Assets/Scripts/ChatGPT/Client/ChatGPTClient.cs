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
        var request = new UnityWebRequest(chatGPTSettings.apiURL, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(
            JsonConvert.SerializeObject(new ChatGPTRequest
            {
                Question = prompt
            }));

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.disposeUploadHandlerOnDispose = true;
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
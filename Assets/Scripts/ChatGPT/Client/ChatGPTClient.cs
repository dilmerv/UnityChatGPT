using DilmerGames.Core.Singletons;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTClient : Singleton<ChatGPTClient>
{
    [SerializeField]
    private ChatGPTSettings chatGPTSettings;

    public UnityWebRequest Request
    {
        get;
        private set;
    }

    public IEnumerator Ask(string prompt, System.Action<ChatGPTResponse> callBack)
    {
        var url = chatGPTSettings.debug ? $"{chatGPTSettings.apiURL}?debug=true" : chatGPTSettings.apiURL;
        Request = new UnityWebRequest(url, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(
            JsonConvert.SerializeObject(new ChatGPTRequest
            {
                Question = prompt
            }));

        Request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        Request.downloadHandler = new DownloadHandlerBuffer();
        Request.disposeUploadHandlerOnDispose = true;
        Request.disposeCertificateHandlerOnDispose = true;
        Request.disposeDownloadHandlerOnDispose = true;

        Request.SetRequestHeader("Content-Type", "application/json");
        
        yield return Request.SendWebRequest();

        if (Request.result == UnityWebRequest.Result.ConnectionError || Request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log(Request.error);
        }
        else
        {
            string responseText = Request.downloadHandler.text;
            var response = (new ChatGPTResponse { Data = responseText }).CodeCleanUp();
            callBack(response);
        }
    }

    private void OnDestroy()
    {
        if (Request != null)
        {
            Request.Dispose();
        }
    }
}
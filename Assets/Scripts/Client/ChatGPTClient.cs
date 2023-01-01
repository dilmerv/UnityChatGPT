using DilmerGames.Core.Singletons;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGPTClient : Singleton<ChatGPTClient>
{
    private const string API_URL = "https://api.openai.com/v1/chat_gpt/messages";
    
    [SerializeField]
    private string apiKey;

    public ChatGPTClient(string apiKey)
    {
        this.apiKey = apiKey;
    }

    public IEnumerator GetResponse(string prompt, System.Action<ChatGPTResponse> callBack)
    {
        var request = new UnityWebRequest(API_URL, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(prompt);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log(request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<ChatGPTResponse>(responseText);
            callBack(response);
        }
    }
}

public class ChatGPTResponse
{
    public string Response { get; set; }
}
using DilmerGames.Core.Singletons;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SketchfabClient : Singleton<SketchfabClient>
{
    // archives_max_size=5000 equal 5MB
    private const string SEARCH_API_URL = "https://api.sketchfab.com/v3/search?type=models&downloadable=true&archives_max_size=20000&q={0}&sort_by=-likeCount";
    private const string DOWNLOAD_API_URL = "https://api.sketchfab.com/v3/models/{0}/download";
    private const string API_KEY = "bb0128d1fcac4979afd63231f771596b";

    public IEnumerator SearchForModels(string modelName, System.Action<SketchfabSearchResponse> callback)
    {
        var url = string.Format(SEARCH_API_URL, modelName);
        UnityWebRequest request = BuildRequest(url);

        // Send request
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError(request.error);
            yield break;
        }

        // Deserialize response
        string responseBody = request.downloadHandler.text;
        var responseData = JsonConvert.DeserializeObject<SketchfabSearchResponse>(responseBody);
        callback(responseData);
    }

    public IEnumerator DownloadModel(string modelId, System.Action<SketchfabDownloadResponse> callback)
    {
        var url = string.Format(DOWNLOAD_API_URL, modelId);
        UnityWebRequest request = BuildRequest(url);

        // Send request
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError(request.error);
            yield break;
        }

        // Deserialize response
        string responseBody = request.downloadHandler.text;
        var responseData = JsonConvert.DeserializeObject<SketchfabDownloadResponse>(responseBody);
        callback(responseData);
    }

    public IEnumerator DownloadZipFile(string url, System.Action<string> callBack)
    {
        // Set up request
        UnityWebRequest request = UnityWebRequest.Get(url);
        string downloadPath = Application.persistentDataPath + "/aaa.zip";
        request.downloadHandler = new DownloadHandlerFile(downloadPath);

        // Send request
        UnityWebRequestAsyncOperation op = request.SendWebRequest();

        while (!op.isDone)
        {

            Debug.Log(request.downloadedBytes / 1000 + "KB");
            yield return null;
        }

        // Check for errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.LogError(request.error);
            yield break;
        }

        callBack(downloadPath);
        
    }
    private UnityWebRequest BuildRequest(string url)
    {
        // Set up request
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("Authorization", $"Token {API_KEY}");
        request.SetRequestHeader("Content-Type", "application/json");

        request.downloadHandler = new DownloadHandlerBuffer();

        return request;
    }
}
using DilmerGames.Core.Singletons;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SketchfabClient : Singleton<SketchfabClient>
{
    [SerializeField]
    private SketchfabSettings sketchfabSettings;

    public IEnumerator SearchForModels(string modelName, System.Action<SketchfabSearchResponse> callback)
    {
        return APICall(sketchfabSettings.searchAPIUrl, modelName, callback);
    }

    public IEnumerator DownloadModel(string modelId, System.Action<SketchfabDownloadResponse> callback)
    {
        return APICall(sketchfabSettings.downloadAPIUrl, modelId, callback);
    }

    public IEnumerator DownloadZipFile(string url, System.Action<string> callBack)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            string downloadPath = Application.persistentDataPath + "/model.zip";
            request.downloadHandler = new DownloadHandlerFile(downloadPath);

            UnityWebRequestAsyncOperation op = request.SendWebRequest();

            while (!op.isDone)
            {
                Debug.Log(request.downloadedBytes / 1000 + "KB");
                yield return null;
            }

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(request.error);
                yield break;
            }
            callBack(downloadPath);
        }
    }

    public IEnumerator APICall<T>(string url, string paramName, System.Action<T> callback)
    {
        var urlWithParams = string.Format(url, paramName);
        using (UnityWebRequest request = BuildRequest(urlWithParams))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(request.error);
                yield break;
            }

            string responseBody = request.downloadHandler.text;
            var responseData = JsonConvert.DeserializeObject<T>(responseBody);
            callback(responseData);
        }
    }

    private UnityWebRequest BuildRequest(string url)
    {
        // Set up request
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", $"Token {sketchfabSettings.apiToken}");
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer();
        return request;
    }
}
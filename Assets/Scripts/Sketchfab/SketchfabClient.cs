using DilmerGames.Core.Singletons;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class SketchfabClient : Singleton<SketchfabClient>
{
    [SerializeField]
    private SketchfabSettings sketchfabSettings;

    public IEnumerator SearchForModels(string modelName, Action<SketchfabSearchResponse> callback)
    {
        return APICall(sketchfabSettings.searchAPIUrl, modelName, callback);
    }

    public IEnumerator DownloadModel(string modelId, Action<SketchfabDownloadResponse> callback)
    {
        return APICall(sketchfabSettings.downloadAPIUrl, modelId, callback);
    }

    public IEnumerator DownloadZipFile(string url, System.Action<string> callBack)
    {
        string downloadPath = $"{Application.persistentDataPath}/models";
        PurgeModels(downloadPath);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            string modelDownloadPath = $"{downloadPath}/model.zip";
            if (File.Exists(modelDownloadPath)) File.Delete(modelDownloadPath);
            request.downloadHandler = new DownloadHandlerFile(modelDownloadPath);

            UnityWebRequestAsyncOperation op = request.SendWebRequest();

            while (!op.isDone)
            {
                ChatGPTProgress.Instance.Status = $"Downloading Model: {(request.downloadedBytes / 1000 + "KB")}";
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

    private void PurgeModels(string directory)
    {
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        Array.ForEach(Directory.GetFiles(directory), f => File.Delete(f));
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
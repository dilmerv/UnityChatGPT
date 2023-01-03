using Newtonsoft.Json;

public class SketchfabSearchResponse
{
    [JsonProperty(PropertyName = "results")]
    public SketchfabSearchResults[] Results;
}

public class SketchfabSearchResults
{
    [JsonProperty(PropertyName = "uid")]
    public string ModelId { get; set; }

    [JsonProperty(PropertyName = "viewerUrl")]
    public string ViewerUrl { get; set; }
}

public class SketchfabDownloadResponse
{
    [JsonProperty(PropertyName = "gltf")]
    public SketchfabDownload Gltf { get; set; }
}

public class SketchfabDownload
{
    [JsonProperty(PropertyName = "url")]
    public string Url { get; set; }
}
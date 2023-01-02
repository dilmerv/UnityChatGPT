
using Newtonsoft.Json;

public class ChatGPTResponse
{
    [JsonProperty(PropertyName = "data")]
    public string Data { get; set; }
}
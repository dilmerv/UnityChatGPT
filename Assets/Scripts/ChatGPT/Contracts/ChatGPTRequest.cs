using Newtonsoft.Json;

public class ChatGPTRequest
{
    [JsonProperty(PropertyName = "question")]
    public string Question { get; set; }
}
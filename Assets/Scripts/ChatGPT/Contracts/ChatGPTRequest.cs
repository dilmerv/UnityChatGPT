using Newtonsoft.Json;

public class ChatGPTRequest
{
    [JsonProperty(PropertyName = "model")]
    public string Model { get; set; }

    [JsonProperty(PropertyName = "messages")]
    public ChatGPTMessage[] Messages { get; set; }
}
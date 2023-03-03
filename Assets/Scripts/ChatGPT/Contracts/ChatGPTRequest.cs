using Newtonsoft.Json;
using System;

public class ChatGPTRequest
{
    [JsonProperty(PropertyName = "model")]
    public string Model { get; set; }

    [JsonProperty(PropertyName = "messages")]
    public ChatGPTMessage[] Messages { get; set; }
}

[Serializable]
public class ChatGPTMessage
{
    public string role;

    public string content;
}
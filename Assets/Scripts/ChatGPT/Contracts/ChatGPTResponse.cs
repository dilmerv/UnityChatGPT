using System;
using System.Collections.Generic;

[Serializable]
public class ChatGPTResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public List<ChatGPTChatChoice> Choices { get; set; }
    public ChatGPTChatUsage Usage { get; set; }
    public double ResponseTotalTime { get; set; }
}
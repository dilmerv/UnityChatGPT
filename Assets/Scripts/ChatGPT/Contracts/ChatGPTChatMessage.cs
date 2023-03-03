using System;

[Serializable]
public class ChatGPTChatMessage
{
    public string Role { get; set; }
    public string Content { get; set; }
}
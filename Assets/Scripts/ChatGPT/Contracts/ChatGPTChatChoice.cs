using System;

[Serializable]
public class ChatGPTChatChoice
{
    public int Index { get; set; }
    public ChatGPTChatMessage Message { get; set; }
    public string FinishReason { get; set; }
}
using System;
using System.Collections.Generic;

[Serializable]
public class ChatGPTResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public long Created { get; set; }
    public List<ChatChoice> Choices { get; set; }
    public ChatUsage Usage { get; set; }
    public double ResponseTotalTime { get; set; }
}

[Serializable]
public class ChatChoice
{
    public int Index { get; set; }
    public ChatMessage Message { get; set; }
    public string FinishReason { get; set; }
}

[Serializable]
public class ChatMessage
{
    public string Role { get; set; }
    public string Content { get; set; }
}

[Serializable]
public class ChatUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}

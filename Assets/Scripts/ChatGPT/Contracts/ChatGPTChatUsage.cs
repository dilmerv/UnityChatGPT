using System;
using UnityEngine;

[Serializable]
public class ChatGPTChatUsage
{
    [field: SerializeField]
    public int PromptTokens { get; set; }

    [field: SerializeField]
    public int CompletionTokens { get; set; }

    [field: SerializeField]
    public int TotalTokens { get; set; }
}

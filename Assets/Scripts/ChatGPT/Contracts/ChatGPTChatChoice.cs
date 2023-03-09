using System;
using UnityEngine;

[Serializable]
public class ChatGPTChatChoice
{
    [field: SerializeField]
    public int Index { get; set; }

    [field: SerializeField]
    public ChatGPTChatMessage Message { get; set; }

    [field: SerializeField]
    public string FinishReason { get; set; }
}
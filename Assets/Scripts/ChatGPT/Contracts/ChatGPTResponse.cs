using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChatGPTResponse
{
    [field: SerializeField]
    public string Id { get; set; }

    [field: SerializeField]
    public string Object { get; set; }

    [field: SerializeField]
    public long Created { get; set; }

    [field: SerializeField]
    public List<ChatGPTChatChoice> Choices { get; set; }

    [field: SerializeField]
    public ChatGPTChatUsage Usage { get; set; }

    [field: SerializeField]
    public double ResponseTotalTime { get; set; }
}
using System;
using UnityEngine;

[Serializable]
public class ChatGPTChatMessage
{
    [field: SerializeField]
    public string Role { get; set; }

    [field: SerializeField]
    public string Content { get; set; }
}
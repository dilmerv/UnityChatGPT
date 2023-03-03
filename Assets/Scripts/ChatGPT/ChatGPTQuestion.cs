using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatGPTQuestion", menuName = "ChatGPT/ChatGPTQuestion", order = 2)]
public class ChatGPTQuestion : ScriptableObject
{
    public string scenarioTitle;

    public string promptPrefixConstant;

    [TextArea(8, 20)]
    public string prompt;

    public ChatGPTReplacement[] replacements;

    public string[] reminders;
}

[Serializable]
public struct ChatGPTReplacement
{
    public Replacements replacementType;

    public string value;
}

[Serializable]
public enum Replacements
{
    CLASS_NAME,
    ACTION,
    API_KEY
}

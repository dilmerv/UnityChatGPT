using UnityEngine;

[CreateAssetMenu(fileName = "ChatGPTQuestion", menuName = "ChatGPT/ChatGPTQuestion", order = 2)]
public class ChatGPTQuestion : ScriptableObject
{
    [TextArea(10,20)]
    public string prompt;

    public ChatGPTReplacement[] replacements;
}

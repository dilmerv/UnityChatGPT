using UnityEngine;

[CreateAssetMenu(fileName = "ChatGPTQuestion", menuName = "ChatGPT/ChatGPTQuestion", order = 2)]
public class ChatGPTQuestion : ScriptableObject
{
    public string scenarioTitle;

    [TextArea(8,20)]
    public string prompt;

    [ReadOnly]
    public string cachedPrompt;

    public ChatGPTReplacement[] replacements;

}

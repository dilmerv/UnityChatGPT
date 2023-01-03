using UnityEngine;

[CreateAssetMenu(fileName = "ChatGPTSettings", menuName = "ChatGPT/ChatGPTSettings", order = 1)]
public class ChatGPTSettings : ScriptableObject
{
    public string apiURL;

    public string apiKey;

    public bool debug;
}

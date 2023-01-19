using UnityEngine;

[CreateAssetMenu(fileName = "ChatGPTSettings", menuName = "ChatGPT/ChatGPTSettings")]
public class ChatGTPSettings : ScriptableObject
{
    public string apiURL;

    public string apiKey;

    public bool debug;

    public string[] reminders;
}

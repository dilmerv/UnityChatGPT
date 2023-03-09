using UnityEngine;

[CreateAssetMenu(fileName = "ChatGPTSettings", menuName = "ChatGPT/ChatGPTSettings")]
public class ChatGTPSettings : ScriptableObject
{
    public string apiURL;

    public string apiKey;

    public string apiOrganization;

    public string apiModel;

    public bool debug;
}

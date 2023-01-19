using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTTester : MonoBehaviour
{
    [SerializeField]
    private Button askButton;

    [SerializeField]
    private TextMeshProUGUI chatGPTAnswer;

    [SerializeField]
    private string prompt;

    public void Execute()
    {
        StartCoroutine(ChatGPTClient.Instance.Ask(prompt, (r) => ProcessResponse(r)));
    }

    public void ProcessResponse(ChatGPTResponse response)
    {
        Logger.Instance.LogInfo(response.Data);
    }
}

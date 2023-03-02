using System;
using System.Linq;
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
    private TextMeshProUGUI chatGPTQuestionText;

    [SerializeField]
    private ChatGPTQuestion chatGPTQuestion;

    private string gptPrompt;

    private string gptAppendedCode;

    [SerializeField]
    private TextMeshProUGUI scenarioTitleText;

    [SerializeField]
    private TextMeshProUGUI scenarioQuestionText;

    public void Execute()
    {
        gptPrompt = chatGPTQuestion.prompt;

        scenarioTitleText.text = chatGPTQuestion.scenarioTitle;

        askButton.interactable = false;

        ChatGPTProgress.Instance.StartProgress("Generating source code please wait");

        // handle replacements
        Array.ForEach(chatGPTQuestion.replacements, r =>
        {
            gptPrompt = gptPrompt.Replace("{" + $"{r.replacementType}" + "}", r.value);
        });

        // handle reminders
        if (chatGPTQuestion.reminders.Length > 0)
        {
            gptPrompt += $", {string.Join(',', chatGPTQuestion.reminders)}";
        }

        scenarioQuestionText.text = gptPrompt;

        StartCoroutine(ChatGPTClient.Instance.Ask(gptPrompt, (r) => ProcessResponse(r)));
    }

    public void ProcessResponse(ChatGPTResponse response)
    {
        Logger.Instance.LogInfo(response.Data);

        string classValue = chatGPTQuestion.replacements
            .FirstOrDefault(r => r.replacementType == Replacements.CLASS_NAME)
            .value ?? string.Empty;

        gptAppendedCode = chatGPTQuestion.codeAppended.Replace("{CLASS_NAME}", classValue);

        RoslynCodeRunner.Instance.AdditionalCode = gptAppendedCode;
        RoslynCodeRunner.Instance.RunCode(response.Data);
    }
}

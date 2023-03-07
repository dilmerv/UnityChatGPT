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
    private Button compilerButton;

    [SerializeField]
    private TextMeshProUGUI responseTimeText;

    [SerializeField]
    private TextMeshProUGUI chatGPTAnswer;

    [SerializeField]
    private TextMeshProUGUI chatGPTQuestionText;

    [SerializeField]
    private ChatGPTQuestion chatGPTQuestion;

    private string gptPrompt;

    [SerializeField]
    private TextMeshProUGUI scenarioTitleText;

    [SerializeField]
    private TextMeshProUGUI scenarioQuestionText;

    [SerializeField]
    private bool immediateCompilation = false;

    [SerializeField]
    private ChatGPTResponse lastChatGPTResponseCache;


    public string ChatGPTMessage
    {
        get
        {
            return (lastChatGPTResponseCache.Choices.FirstOrDefault()?.Message?.Content ?? null) ?? string.Empty;
        }
    }

    public Color CompileButtonColor
    {
        set
        {
            compilerButton.GetComponent<Image>().color = value;
        }
    }

    private void Awake()
    {
        responseTimeText.text = string.Empty;
        compilerButton.interactable = false;

        askButton.onClick.AddListener(() =>
        {
            compilerButton.interactable = false;
            CompileButtonColor = Color.white;

            Execute();
        });
    }

    public void Execute()
    {
        gptPrompt = $"{chatGPTQuestion.promptPrefixConstant} {chatGPTQuestion.prompt}";

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

        StartCoroutine(ChatGPTClient.Instance.Ask(gptPrompt, (response) =>
        {
            askButton.interactable = true;

            CompileButtonColor = Color.green;

            compilerButton.interactable = true;
            lastChatGPTResponseCache = response;
            responseTimeText.text = $"Time: {response.ResponseTotalTime} ms";

            ChatGPTProgress.Instance.StopProgress();

            Logger.Instance.LogInfo(ChatGPTMessage);

            if (immediateCompilation)
                ProcessAndCompileResponse();
        }));
    }

    public void ProcessAndCompileResponse()
    {
        RoslynCodeRunner.Instance.RunCode(ChatGPTMessage);
    }
}

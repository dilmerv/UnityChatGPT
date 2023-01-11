using GLTFast;
using RoslynCSharp;
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
    private Button importModelButton;

    [SerializeField]
    private TextMeshProUGUI scenarioTitle;

    [SerializeField]
    private TextMeshProUGUI scenarioQuestion;

    [HideInInspector]
    private ChatGPTQuestion cachedChatGPTQuestion;

    [SerializeField]
    private ChatGPTQuestion chatGPTQuestion;

    private string gptPrompt;

    private ScriptDomain domain = null;

    private int attemptsAllowed = 3;

    private int attemptsCount = 0;

    private void Awake()
    {
        domain = ScriptDomain.CreateDomain(nameof(ChatGPTTester));
        scenarioTitle.text = string.Empty;

        //cached initial question
        cachedChatGPTQuestion = chatGPTQuestion;
    }

    /// <summary>
    /// Handles cases when question changes and we need to cache the original
    /// question asked, this allow us to rerun the same question multiple times
    /// </summary>
    private void OnValidate()
    {
        if(cachedChatGPTQuestion != chatGPTQuestion)
        {
            Debug.Log($"Question is changed to scenario: {chatGPTQuestion.scenarioTitle}");
            cachedChatGPTQuestion = chatGPTQuestion;
        }
    }

    /// <summary>
    /// Execute is called from the AskButton labeled "ASK CHATGPT"
    /// </summary>
    public void Execute()
    {
        gptPrompt = chatGPTQuestion.prompt;

        // populate scenario question
        scenarioTitle.text = $"Scenario Question: {chatGPTQuestion.scenarioTitle}";

        askButton.interactable = importModelButton.interactable = false;

        ChatGPTProgress.Instance.StartProgress($"Generating code (attempt #{attemptsCount+1}) please wait");

        // handle replacements
        Array.ForEach(chatGPTQuestion.replacements, r =>
        {
            gptPrompt = gptPrompt.Replace("{" + $"{r.replacementType}" + "}", r.value);
        });

        scenarioQuestion.text = gptPrompt;

        // call chatGPT service
        StartCoroutine(ChatGPTClient.Instance.Ask(gptPrompt, (r) => ProcessResponse(r)));
    }

    public void ImportModel(string modelName)
    {
        ChatGPTProgress.Instance.StartProgress($"Importing model {modelName} please wait");
        StartCoroutine(SketchfabClient.Instance.SearchForModels(modelName, (r) => ProcessResponseModel(r)));
    }

    private void ProcessResponseModel(SketchfabSearchResponse response)
    {
        var modelCount = response.Results.Count();
        if (modelCount == 0 || response.Results == null) return;

        var pickAModelAtIndex = UnityEngine.Random.Range(0, modelCount);
        var model = response.Results.Skip(pickAModelAtIndex - 1).Take(1).FirstOrDefault();

        if (model != null)
        {
            StartCoroutine(SketchfabClient.Instance.DownloadModel(model.ModelId, (r) => ProcessDownloadModel(r)));
        }
    }

    private void ProcessDownloadModel(SketchfabDownloadResponse response)
    {
        StartCoroutine(SketchfabClient.Instance.DownloadZipFile(response.Gltf.Url,
            (r) => (new GltfImport()).ExtractAndImportGLTF(r, chatGPTQuestion.SearchEntityValue)));
    }

    private void ProcessResponse(ChatGPTResponse response)
    {   
        askButton.interactable = importModelButton.interactable = true;
        ChatGPTProgress.Instance.StopProgress();
        Logger.Instance.LogInfo(response.Data);

        try
        {
            // Compile and load the source code
            ScriptAssembly assembly = domain.CompileAndLoadSource(response.Data, ScriptSecurityMode.UseSettings);

            ScriptType behaviourType = assembly.FindSubTypeOf<MonoBehaviour>(chatGPTQuestion.replacements
                .FirstOrDefault(r => r.replacementType == Replacements.CLASS_NAME).value);

            ScriptProxy proxy = behaviourType.CreateInstance(gameObject);

            // add an optional check here
            proxy.Call(chatGPTQuestion.replacements
                .FirstOrDefault(r => r.replacementType == Replacements.ACTION_APPLY).value);
        }
        catch (Exception e)
        {
            Logger.Instance.LogWarning($"Review the generated code, more likely ChatGPT\ndidn't produce the prompt exactly as indicated.\n{e.Message}");
            // if we get an error let's try to tell ChatGPT what happened and see
            // if is smart enough to figure it out.
            if (attemptsCount < attemptsAllowed)
            {
                attemptsCount++;
                //TODO implement a good way of avoiding future errors
                //chatGPTQuestion.prompt += $", avoid this error: {e.Message}";
                Execute();
            }
        }
    }

    private void OnDestroy()
    {
        if (domain != null)
        {
            domain.Dispose();
        }
    }
}

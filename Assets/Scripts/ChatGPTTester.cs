using GLTFast;
using RoslynCSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTTester : MonoBehaviour
{
    [SerializeField]
    private Button askButton;

    [SerializeField]
    [TextArea(8, 10)]
    private string prompt;

    private string cachedPrompt;

    [SerializeField]
    private ChatGPTReplacement[] replacements;

    private ScriptDomain domain = null;

    private int attemptsAllowed = 3;

    private int attemptsCount = 0;

    private void Awake()
    {
        domain = ScriptDomain.CreateDomain(nameof(ChatGPTTester));
        cachedPrompt = prompt;
    }

    /// <summary>
    /// Execute is called from the AskButton labeled "ASK CHATGPT"
    /// </summary>
    public void Execute()
    {
        // restore cached - userful for running multiple times
        prompt = cachedPrompt;

        askButton.interactable = false;

        ChatGPTProgress.Instance.StartProgress($"Generating code (attempt #{attemptsCount+1}) please wait");

        // handle replacements
        Array.ForEach(replacements, r =>
        {
            prompt = prompt.Replace("{" + $"{r.replacementType}" + "}", r.value);
        });

        // call chatGPT service
        StartCoroutine(ChatGPTClient.Instance.Ask(prompt, (r) => ProcessResponse(r)));
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
            (r) => (new GltfImport()).ExtractAndImportGLTF(r)));
    }


    private void ProcessResponse(ChatGPTResponse response)
    {   
        askButton.interactable = true;
        ChatGPTProgress.Instance.StopProgress();
        Logger.Instance.LogInfo(response.Data);

        try
        {
            // Compile and load the source code
            ScriptAssembly assembly = domain.CompileAndLoadSource(response.Data, ScriptSecurityMode.UseSettings);

            ScriptType behaviourType = assembly.FindSubTypeOf<MonoBehaviour>(replacements
                .FirstOrDefault(r => r.replacementType == Replacements.CLASS_NAME).value);

            ScriptProxy proxy = behaviourType.CreateInstance(gameObject);

            // add an optional check here
            proxy.Call(replacements
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
                cachedPrompt += $", avoid this error: {e.Message}";
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

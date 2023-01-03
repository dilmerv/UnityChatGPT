using GLTFast;
using RoslynCSharp;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTTester : MonoBehaviour
{
    [SerializeField]
    private Button askButton;

    [SerializeField]
    [TextArea(8, 10)]
    private string prompt;

    [SerializeField]
    private ChatGPTReplacement[] replacements;

    private ScriptDomain domain = null;

    void Awake()
    {
        domain = ScriptDomain.CreateDomain(nameof(ChatGPTTester));
    }

    public void ImportModel(string modelName)
    {
        ChatGPTProgress.Instance.StartProgress(@"Importing model {modelName} please wait");
        StartCoroutine(SketchfabClient.Instance.SearchForModels(modelName, (r) => ProcessResponseModel(r)));
    }

    void ProcessResponseModel(SketchfabSearchResponse response)
    {
        var grabTopModel = response.Results.FirstOrDefault();
        if (grabTopModel != null)
        {
            StartCoroutine(SketchfabClient.Instance.DownloadModel(grabTopModel.ModelId, (r) => ProcessDownloadModel(r)));
        }
    }

    void ProcessDownloadModel(SketchfabDownloadResponse response)
    {
        StartCoroutine(SketchfabClient.Instance.DownloadZipFile(response.Gltf.Url, (r) => ProcessZipfile(r)));
    }

    async void ProcessZipfile(string zipfilePath)
    {
        try
        {
            var targetDirectory = Path.GetDirectoryName(zipfilePath);
            ZipFile.ExtractToDirectory(zipfilePath, targetDirectory, true);

            var gltf = new GltfImport();
            // Create a settings object and configure it accordingly
            var settings = new ImportSettings
            {
                GenerateMipMaps = true,
                AnisotropicFilterLevel = 3,
                NodeNameMethod = NameImportMethod.OriginalUnique
            };

            // Load the glTF and pass along the settings
            var success = await gltf.Load($"{targetDirectory}\\scene.gltf", settings);

            if (success)
            {
                var gameObject = new GameObject("glTF");
                await gltf.InstantiateMainSceneAsync(gameObject.transform);
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
        }
        catch(Exception e)
        {
            Logger.Instance.LogError(e.ToString());
        }
        ChatGPTProgress.Instance.StopProgress();
    }
   

    /// <summary>
    /// Execute is called from the AskButton labeled "ASK CHATGPT"
    /// </summary>
    public void Execute()
    {
        askButton.interactable = false;

        ChatGPTProgress.Instance.StartProgress("Generating code please wait");
        
        // handle replacements
        Array.ForEach(replacements, r => 
        { 
            prompt = prompt.Replace("{" + $"{r.replacementType}" + "}", r.value); 
        });

        // call chatGPT service
        StartCoroutine(ChatGPTClient.Instance.Ask(prompt, (r) => ProcessResponse(r)));
    }

    void ProcessResponse(ChatGPTResponse response)
    {
        askButton.interactable = true;
        ChatGPTProgress.Instance.StopProgress();

        Logger.Instance.LogInfo(response.Data);

        // Compile and load the source code
        ScriptAssembly assembly = domain.CompileAndLoadSource(response.Data, ScriptSecurityMode.UseSettings);

        ScriptType behaviourType = assembly.FindSubTypeOf<MonoBehaviour>(replacements
            .FirstOrDefault(r => r.replacementType == Replacements.CLASS_NAME).value);

        ScriptProxy proxy = behaviourType.CreateInstance(gameObject);

        // add an optional check here
        proxy.Call(replacements
            .FirstOrDefault(r => r.replacementType == Replacements.ACTION_APPLY).value);
    }

    void OnDestroy()
    {
        if (domain != null)
        {
            domain.Dispose();
        }
    }
}

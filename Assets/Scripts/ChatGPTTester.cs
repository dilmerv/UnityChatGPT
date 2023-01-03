using RoslynCSharp;
using System;
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

    private void Awake()
    {
        domain = ScriptDomain.CreateDomain(nameof(ChatGPTTester));
    }

    public void Execute()
    {
        askButton.interactable = false;
        ChatGPTProgress.Instance.StartProgress();
        
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

        proxy.Call(replacements
            .FirstOrDefault(r => r.replacementType == Replacements.ACTION).value);
    }

    void OnDestroy()
    {
        if (domain != null)
        {
            domain.Dispose();
        }
    }
}

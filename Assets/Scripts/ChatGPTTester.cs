using RoslynCSharp;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTTester : MonoBehaviour
{
    [SerializeField]
    private Button askButton;

    [SerializeField]
    [TextArea(5, 10)]
    private string prompt;

    private ScriptDomain domain = null;

    private void Awake()
    {
        domain = ScriptDomain.CreateDomain("MyTestDomain");
    }

    public void Execute()
    {
        askButton.interactable = false;
        ChatGPTProgress.Instance.StartProgress();
        StartCoroutine(ChatGPTClient.Instance.Ask(prompt, (r) => ProcessResponse(r)));
    }

    void ProcessResponse(ChatGPTResponse response)
    {
        askButton.interactable = true;
        ChatGPTProgress.Instance.StopProgress();

        Logger.Instance.LogInfo(response.Data);

        // Compile and load the source code
        ScriptAssembly assembly = domain.CompileAndLoadSource(response.Data, ScriptSecurityMode.UseSettings);

        ScriptType behaviourType = assembly.FindSubTypeOf<MonoBehaviour>("CubePlacer");

        ScriptProxy proxy = behaviourType.CreateInstance(gameObject);

        proxy.Call("Apply");
    }
}

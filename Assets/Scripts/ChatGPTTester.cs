using RoslynCSharp;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTTester : MonoBehaviour
{
    [SerializeField]
    private Button askButton;

    [SerializeField]
    [TextArea(3, 10)]
    private string prompt;

    private ScriptDomain domain = null;

    private void Awake()
    {
        domain = ScriptDomain.CreateDomain("MyTestDomain");
    }

    public void Execute()
    {
        askButton.interactable = false;
        StartCoroutine(ChatGPTClient.Instance.Ask(prompt, (r) => ProcessResponse(r)));
    }

    void ProcessResponse(ChatGPTResponse response)
    {
        askButton.interactable = true;

        Logger.Instance.Clear();
        Logger.Instance.LogInfo(response.Data);

        // Compile and load the source code
        ScriptAssembly assembly = domain.CompileAndLoadSource(response.Data, ScriptSecurityMode.UseSettings);

        ScriptType behaviourType = assembly.FindSubTypeOf<MonoBehaviour>("CubePlacer");

        ScriptProxy proxy = behaviourType.CreateInstance(gameObject);

        proxy.Call("Apply");
    }
}

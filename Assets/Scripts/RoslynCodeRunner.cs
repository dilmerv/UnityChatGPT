using DilmerGames.Core.Singletons;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoslynCodeRunner : Singleton<RoslynCodeRunner>
{
    [SerializeField]
    private string[] namespaces;

    [SerializeField]
    [TextArea(5, 12)]
    private string code;

    public string AdditionalCode { get; set; }

    [SerializeField]
    private UnityEvent OnRunCodeCompleted;

    [SerializeField]
    private string[] resultVars;

    [SerializeField]
    [TextArea(5, 20)]
    private string resultInfo;

    public void RunCode(string updatedCode = null)
    {
        Logger.Instance.LogInfo("Executing Runcode...");
        updatedCode = string.IsNullOrEmpty(updatedCode) ? null : updatedCode;
        try
        {
            code = $"{(updatedCode ?? code)} {AdditionalCode}";
            ScriptState<object> result = CSharpScript.RunAsync(code, SetDefaultImports()).Result;

            foreach(string var in resultVars)
            {
                resultInfo += $"{result.GetVariable(var).Name}: {result.GetVariable(var).Value}\n";
            }

            OnRunCodeCompleted?.Invoke();
        }
        catch(Exception e)
        {
            Logger.Instance.LogError(e.Message);
        }
    }

    private ScriptOptions SetDefaultImports()
    {
        return ScriptOptions.Default
            .WithImports(namespaces.Select(n => n.Replace("using", string.Empty)
            .Trim()))
            .AddReferences(
                typeof(MonoBehaviour).Assembly,
                typeof(Debug).Assembly
            );
    }
}

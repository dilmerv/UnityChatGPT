using DilmerGames.Core.Singletons;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using StarterAssets;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RoslynCodeRunner : Singleton<RoslynCodeRunner>
{
    [SerializeField]
    private string[] namespaces;

    [SerializeField]
    public string codeExecutionGameObjectName;

    [SerializeField]
    [TextArea(15, 35)]
    private string code;

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
            code = $"{(updatedCode ?? code)}";

            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var firstMember = root.Members[0];
            var classInfo = (ClassDeclarationSyntax)firstMember;

            var addedCodeForGOExecution = $"GameObject.Find(\"{codeExecutionGameObjectName}\").AddComponent<{classInfo.Identifier.Value}>();";

            ScriptState<object> result = CSharpScript.RunAsync($"{code} {addedCodeForGOExecution}", SetDefaultImports()).Result;
            
            foreach (string var in resultVars)
            {
                resultInfo += $"{result.GetVariable(var).Name}: {result.GetVariable(var).Value}\n";
            }

            OnRunCodeCompleted?.Invoke();
        }
        catch(Exception mainCodeException)
        {
            Logger.Instance.LogError(mainCodeException.Message);
        }
    }

    private ScriptOptions SetDefaultImports()
    {
        return ScriptOptions.Default
            .WithImports(namespaces.Select(n => n.Replace("using", string.Empty)
            .Trim()))
            // TODO - make these configurable instead of having to add each reference manually
            .AddReferences(
                typeof(MonoBehaviour).Assembly,
                typeof(Debug).Assembly,
                typeof(TextMeshPro).Assembly,
                typeof(IEnumerator).Assembly,
                typeof(StarterAssetsInputs).Assembly
            );
    }
}

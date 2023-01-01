using RoslynCSharp;
using System.Collections;
using UnityEngine;

public class DynamicCode : MonoBehaviour
{
    private ScriptDomain domain = null;

    // The C# source code we will load
    private string source = @"
     using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    public void Apply()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = transform.position + new Vector3(Random.Range(-2f, 2f), 0, 0);
        }
    }
}";

    void Start()
    {
        StartCoroutine(ChatGPTClient.Instance
            .GetResponse("Create a MonoBehaviour called CubePlacer which has a method called Apply() " +
            "and it creates 20 cubes 2 meters away from the camera and along the x axis separated by 0.1 meter each", (r) => ProcessResponse(r)));

        domain = ScriptDomain.CreateDomain("MyTestDomain");

        LoadAssembly();

        StartCoroutine(DoSomething());
    }

    void ProcessResponse(ChatGPTResponse response)
    { 
    
    }

    private void LoadAssembly()
    {
        // Compile and load the source code
        ScriptAssembly assembly = domain.CompileAndLoadSource(source, ScriptSecurityMode.UseSettings);


        ScriptType behaviourType = assembly.FindSubTypeOf<MonoBehaviour>("CubePlacer");

        ScriptProxy proxy = behaviourType.CreateInstance(gameObject);
        proxy.Call("Apply");
    }

    IEnumerator DoSomething()
    {
        yield return new WaitForSeconds(5);
        LoadAssembly();
    }

}

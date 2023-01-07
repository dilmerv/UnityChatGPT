using GLTFast;
using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public static class GltfExtensions
{
    public static async void ExtractAndImportGLTF(this GltfImport gltfImport, string zipfilePath)
    {
        try
        {
            var targetDirectory = Path.GetDirectoryName(zipfilePath);
            ZipFile.ExtractToDirectory(zipfilePath, targetDirectory, true);

            // Create a settings object and configure it accordingly
            var settings = new ImportSettings
            {
                GenerateMipMaps = true,
                AnisotropicFilterLevel = 3,
                NodeNameMethod = NameImportMethod.OriginalUnique
            };

            // Load the glTF and pass along the settings
            var success = await gltfImport.Load($"{Path.Combine(targetDirectory,"scene.gltf")}", settings);

            if (success)
            {
                var existingGameObject = GameObject.Find("glTF");
                if (existingGameObject != null) GameObject.DestroyImmediate(existingGameObject);
                var gameObject = new GameObject("glTF");
                await gltfImport.InstantiateMainSceneAsync(gameObject.transform);
            }
            else
            {
                Debug.LogError("Loading glTF failed!");
            }
        }
        catch (Exception e)
        {
            Logger.Instance.LogError(e.ToString());
        }

        ChatGPTProgress.Instance.StopProgress();
    }
}

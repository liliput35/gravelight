using UnityEngine;
using System.IO;

public class TerrainTextureSaver : MonoBehaviour
{
    [SerializeField] private string saveFolderName = "SavedTerrainTextures";
    public Terrain terrain;

    void Start()
    {
        
        if (terrain == null)
        {
            Debug.LogError("No Terrain component found on this GameObject!");
            return;
        }

        TerrainLayer[] layers = terrain.terrainData.terrainLayers;
        if (layers == null || layers.Length == 0)
        {
            Debug.LogWarning("No terrain layers found to export!");
            return;
        }

        // Create folder inside Assets
        string folderPath = Path.Combine(Application.dataPath, saveFolderName);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        foreach (var layer in layers)
        {
            Texture2D tex = layer.diffuseTexture;
            if (tex == null)
            {
                Debug.LogWarning($"Layer '{layer.name}' has no diffuse texture.");
                continue;
            }

            // Duplicate texture to make it readable if needed
            RenderTexture rt = RenderTexture.GetTemporary(tex.width, tex.height, 0);
            Graphics.Blit(tex, rt);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D readableTex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
            readableTex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            readableTex.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);

            // Encode to PNG
            byte[] pngData = readableTex.EncodeToPNG();
            string filePath = Path.Combine(folderPath, $"{layer.name}.png");
            File.WriteAllBytes(filePath, pngData);

            Debug.Log($" Saved: {filePath}");
        }

        Debug.Log(" Terrain layer textures exported!");
    }
}

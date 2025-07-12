using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ColorSwapTable", menuName = "Color Palettes/Color Swap Table")]
public class ColorSwapTable : ScriptableObject
{
    [TableList(ShowIndexLabels = true)]
    public List<ColorPair> colorPairs = new List<ColorPair>();

    [System.Serializable]
    public class ColorPair
    {
        public Color sourceColor;
        public Color targetColor;
    }

    [Button("Export to Texture")]
    public Texture2D ExportToTexture()
    {
        if (colorPairs.Count == 0) return null;

        Texture2D texture = new Texture2D(colorPairs.Count, 2, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        for (int i = 0; i < colorPairs.Count; i++)
        {
            texture.SetPixel(i, 0, colorPairs[i].sourceColor);
            texture.SetPixel(i, 1, colorPairs[i].targetColor);
        }

        texture.Apply();
        return texture;
    }

    [Button("Import from Texture")]
    public void ImportFromTexture(Texture2D texture)
    {
        if (texture == null) return;

        colorPairs.Clear();
        Color[] pixels = texture.GetPixels();

        int width = texture.width;
        if (texture.height < 2) return;

        for (int x = 0; x < width; x++)
        {
            colorPairs.Add(new ColorPair
            {
                sourceColor = pixels[x],
                targetColor = pixels[x + width]
            });
        }
    }
}
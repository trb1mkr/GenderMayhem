using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRecolor : MonoBehaviour
{
    [System.Serializable]
    public class PaletteData
    {
        [HorizontalGroup("Split", 0.6f)]
        [AssetsOnly]
        public ColorSwapTable palette;

        [HorizontalGroup("Split", 0.4f)]
        [PreviewField(50, ObjectFieldAlignment.Right)]
        public Texture2D maskTexture;

        [Range(0, 1), HideLabel]
        [HorizontalGroup("Split", 0.2f)]
        public float maskThreshold = 0.5f;
    }

    [ListDrawerSettings(ShowIndexLabels = true, Expanded = true)]
    [SerializeField] private List<PaletteData> _palettes = new List<PaletteData>();

    private SpriteRenderer _spriteRenderer;
    private Texture2D _originalTexture;
    private Color[] _originalPixels;
    private Sprite _originalSprite;
    private string _originalSpriteName;
    private Sprite[] _originalSpriteSheet;
    private bool _isMultipleMode;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        CacheOriginalSprite();
        ApplyAllPalettes();
    }

    private void CacheOriginalSprite()
    {
        if (_spriteRenderer.sprite == null) return;

        _originalSprite = _spriteRenderer.sprite;
        _originalSpriteName = _originalSprite.name;

        // Проверяем режим Multiple
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(_originalSprite);
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        _isMultipleMode = importer != null && importer.spriteImportMode == SpriteImportMode.Multiple;
#endif

        if (_isMultipleMode)
        {
#if UNITY_EDITOR
            // Загружаем все спрайты из атласа
            _originalSpriteSheet = AssetDatabase.LoadAllAssetsAtPath(path)
                .Where(x => x is Sprite)
                .Cast<Sprite>()
                .ToArray();
#endif
        }

        // Создаем копию оригинальной текстуры
        var sourceTex = _originalSprite.texture;
        _originalTexture = new Texture2D(
            sourceTex.width,
            sourceTex.height,
            sourceTex.format,
            sourceTex.mipmapCount > 1)
        {
            filterMode = sourceTex.filterMode,
            wrapMode = sourceTex.wrapMode
        };

        Graphics.CopyTexture(sourceTex, _originalTexture);
        _originalPixels = _originalTexture.GetPixels();
    }

    [Button("Apply Palettes")]
    public void ApplyAllPalettes()
    {
        if (_spriteRenderer.sprite == null || _palettes.Count == 0) return;

        Texture2D modifiedTexture = CreateModifiedTexture();
        UpdateSprite(modifiedTexture);
    }

    private Texture2D CreateModifiedTexture()
    {
        Texture2D modifiedTexture = new Texture2D(
            _originalTexture.width,
            _originalTexture.height,
            _originalTexture.format,
            _originalTexture.mipmapCount > 1)
        {
            filterMode = _originalTexture.filterMode,
            wrapMode = _originalTexture.wrapMode
        };

        Color[] currentPixels = _originalTexture.GetPixels();

        foreach (var paletteData in _palettes)
        {
            if (paletteData.palette == null) continue;

            ApplyPaletteWithMask(
                ref currentPixels,
                paletteData.palette.colorPairs,
                paletteData.maskTexture,
                paletteData.maskThreshold);
        }

        modifiedTexture.SetPixels(currentPixels);
        modifiedTexture.Apply();
        return modifiedTexture;
    }

    private void ApplyPaletteWithMask(
        ref Color[] pixels,
        List<ColorSwapTable.ColorPair> colorPairs,
        Texture2D maskTexture,
        float threshold)
    {
        bool useMask = maskTexture != null && 
                      maskTexture.width == _originalTexture.width && 
                      maskTexture.height == _originalTexture.height;

        Color[] maskPixels = useMask ? maskTexture.GetPixels() : null;

        // Если это Multiple режим, применяем изменения только в области текущего спрайта
        Rect spriteRect = _originalSprite.rect;
        int texWidth = _originalTexture.width;

        for (int y = (int)spriteRect.y; y < (int)spriteRect.yMax; y++)
        {
            for (int x = (int)spriteRect.x; x < (int)spriteRect.xMax; x++)
            {
                int pixelIndex = y * texWidth + x;

                if (useMask && maskPixels[pixelIndex].grayscale < threshold) continue;

                foreach (var pair in colorPairs)
                {
                    if (ColorsApproximatelyEqual(pixels[pixelIndex], pair.sourceColor))
                    {
                        pixels[pixelIndex] = pair.targetColor;
                        break;
                    }
                }
            }
        }
    }

    private void UpdateSprite(Texture2D newTexture)
    {
        if (_isMultipleMode)
        {
            UpdateMultipleModeSprite(newTexture);
        }
        else
        {
            UpdateSingleSprite(newTexture);
        }
    }

    private void UpdateSingleSprite(Texture2D newTexture)
    {
        Sprite newSprite = Sprite.Create(
            newTexture,
            _originalSprite.rect,
            _originalSprite.pivot,
            _originalSprite.pixelsPerUnit,
            0,
            SpriteMeshType.Tight,
            _originalSprite.border);

        newSprite.name = _originalSpriteName;
        _spriteRenderer.sprite = newSprite;
    }

    private void UpdateMultipleModeSprite(Texture2D newTexture)
    {
        // Создаем все спрайты из атласа
        List<Sprite> newSprites = new List<Sprite>();

        foreach (var original in _originalSpriteSheet)
        {
            Sprite newSprite = Sprite.Create(
                newTexture,
                original.rect,
                original.pivot,
                original.pixelsPerUnit,
                0,
                SpriteMeshType.Tight,
                original.border);

            newSprite.name = original.name;
            newSprites.Add(newSprite);
        }

        // Находим текущий спрайт в коллекции
        var currentSprite = newSprites.Find(s => s.name == _originalSprite.name);
        if (currentSprite != null)
        {
            _spriteRenderer.sprite = currentSprite;
        }
    }

    [Button("Save Current Sprite")]
    public void SaveCurrentSprite(string path = "Assets/RecoloredSprites/")
    {
        if (_spriteRenderer.sprite == null) return;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fullPath = Path.Combine(path, $"{_originalSpriteName}_Recolored.png");
        byte[] bytes = _spriteRenderer.sprite.texture.EncodeToPNG();
        File.WriteAllBytes(fullPath, bytes);

#if UNITY_EDITOR
        AssetDatabase.Refresh();

        // Для Multiple режима нужно переимпортировать текстуру с правильными настройками
        if (_isMultipleMode)
        {
            var importer = AssetImporter.GetAtPath(fullPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritesheet = GetOriginalSpriteSheetData();
                importer.SaveAndReimport();
            }
        }
#endif

        Debug.Log($"Sprite saved to: {fullPath}");
    }

#if UNITY_EDITOR
    private SpriteMetaData[] GetOriginalSpriteSheetData()
    {
        string path = AssetDatabase.GetAssetPath(_originalSprite);
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        return importer != null ? importer.spritesheet : null;
    }
#endif

    private bool ColorsApproximatelyEqual(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) < 0.01f &&
               Mathf.Abs(a.g - b.g) < 0.01f &&
               Mathf.Abs(a.b - b.b) < 0.01f &&
               Mathf.Abs(a.a - b.a) < 0.01f;
    }

    private void OnDestroy()
    {
        RestoreOriginalSprite();
    }

    [Button("Restore Original")]
    public void RestoreOriginalSprite()
    {
        if (_spriteRenderer != null && _originalTexture != null)
        {
            if (_isMultipleMode && _originalSpriteSheet != null)
            {
                var original = System.Array.Find(_originalSpriteSheet, s => s.name == _originalSpriteName);
                if (original != null) _spriteRenderer.sprite = original;
            }
            else
            {
                Texture2D originalTextureCopy = new Texture2D(
                    _originalTexture.width,
                    _originalTexture.height,
                    _originalTexture.format,
                    _originalTexture.mipmapCount > 1);

                Graphics.CopyTexture(_originalTexture, originalTextureCopy);
                UpdateSingleSprite(originalTextureCopy);
            }
        }
    }

    public void AddPalette(PaletteData newPalette)
    {
        _palettes.Add(newPalette);
        ApplyAllPalettes();
    }

    public void RemovePalette(int index)
    {
        if (index >= 0 && index < _palettes.Count)
        {
            _palettes.RemoveAt(index);
            ApplyAllPalettes();
        }
    }

    [Button("Clear All Palettes")]
    public void ClearAllPalettes()
    {
        _palettes.Clear();
        RestoreOriginalSprite();
    }
}
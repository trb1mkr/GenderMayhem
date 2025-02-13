using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Renamer))]
public class RenamerEditor : Editor
{
    public void Reset()
    {
        Rename();
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Õ≈ «¿ –€¬¿“‹");
        Rename();
    }

    void Rename()
    {
        Renamer gameObject = (Renamer)target;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite != null) gameObject.gameObject.name = spriteRenderer.sprite.name;
    }
}

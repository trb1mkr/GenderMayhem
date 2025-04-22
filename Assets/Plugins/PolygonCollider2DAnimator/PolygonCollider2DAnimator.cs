using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;

public class PolygonCollider2DAnimator : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _colliderComponent;
    public int CurrentCollider;
    public List<PC2DPaths> Colliders = new List<PC2DPaths>();
    [SerializeField][PropertyOrder(1)][FoldoutGroup("View", 0)] private bool _autoUpdateColliderComponent = true;
    [FoldoutGroup("Create", 1)] public string SerializedColliderName;

    [InfoBox("If Auto Save is enabled and Save Folder is changed, use of Update Current Collider will produce new files at new path")]
    [SerializeField][PropertyOrder(1)][FoldoutGroup("Save", 2)] private bool _autoSave = true;
    [SerializeField][FolderPath][PropertyOrder(1)][FoldoutGroup("Save")] private string _saveFolder;

    private void FixedUpdate()
    {
        UpdateColliderComponent();
    }

    private PC2DPaths GetCollider()
    {
        var colliderPaths = ScriptableObject.CreateInstance<PC2DPaths>();
        colliderPaths.name = SerializedColliderName;
        for (int i = 0; i < _colliderComponent.pathCount; i++)
        {
            var path = ScriptableObject.CreateInstance<PC2DPathPoints>();
            path.Points = _colliderComponent.GetPath(i);
            path.name = i.ToString();
            colliderPaths.Paths.Add(path);
        }
        return colliderPaths;
    }

#if UNITY_EDITOR
    private string GetUniqueName(PC2DPaths collider)
    {
        collider.name = collider.name == "" ? Colliders.Count.ToString() : collider.name;

        if (Colliders.Any(x => x.name == collider.name))
        {
            int increment = 1;
            while (Colliders.Any(x => x.name == collider.name + increment)) increment++;
            collider.name += increment;
        }

        return collider.name;
    }

    [Button][FoldoutGroup("Create")]
    private void AddCollider()
    {
        var collider = GetCollider();
        collider.name = GetUniqueName(collider);
        Colliders.Add(collider);
        if (_autoSave) SaveColliderToAsset(Colliders.Count - 1);
    } 

    [Button][FoldoutGroup("Create")]
    private void UpdateCurrentCollider()
    {
        var collider = GetCollider();
        collider.name = Colliders[CurrentCollider].name;
        Colliders[CurrentCollider] = collider;
        if (!AssetDatabase.AssetPathExists($"{_saveFolder}/{collider.name}.asset") && _autoSave) SaveColliderToAsset(Colliders.Count - 1);
    }

    [InfoBox("Update Current Collider Name will not change name of an .asset file if it saved on disk. It can cause errors")]
    [Button][FoldoutGroup("Create")]
    private void UpdateCurrentColliderName() => Colliders[CurrentCollider].name = SerializedColliderName;

    private void SaveColliderToAsset(int index)
    {
        var assetName = Colliders[index].name;
        var savePath = $"{_saveFolder}/{assetName}.asset";

        if (AssetDatabase.AssetPathExists(savePath))
        {
            Debug.LogWarning($"Asset {assetName} already exists in {_saveFolder}. Save aborted.");
            return;
        }

        AssetDatabase.CreateAsset(Colliders[index], savePath);
        for (int i = 0; i < Colliders[index].Paths.Count; i++)
            AssetDatabase.AddObjectToAsset(Colliders[index].Paths[i], savePath);

        AssetDatabase.SaveAssets();
    }

    [Button][PropertyOrder(1)][FoldoutGroup("Save")]
    private void SaveCurrentColliderFromList()
    {
        SaveColliderToAsset(CurrentCollider);
    }

    [Button][PropertyOrder(1)][FoldoutGroup("Save")]
    private void SaveAllCollidersFromList()
    {
        for (int i = 0; i < Colliders.Count; i++)
            SaveColliderToAsset(i);
    }
#endif

    [OnInspectorGUI]
    private void AutoUpdateColliderComponent()
    {
        if (_autoUpdateColliderComponent) UpdateColliderComponent();
    }

    [Button][FoldoutGroup("View")]
    public void UpdateColliderComponent()
    {
        if (Colliders.Count - 1 < CurrentCollider || Colliders[CurrentCollider] == null) return;
        _colliderComponent.points = new Vector2[] {};
        if (Colliders[CurrentCollider].Paths == null) Debug.LogError($"Paths subasset is null for some reason. Fix it.");
        for (int i = 0; i < Colliders[CurrentCollider].Paths.Count; i++)
            _colliderComponent.SetPath(i, Colliders[CurrentCollider].Paths[i].Points);
    }
}

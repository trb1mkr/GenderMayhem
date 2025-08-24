using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using R3;

public class PolygonCollider2DAnimator : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _colliderComponent;
    public int CurrentCollider;
    public List<PC2DPaths> Colliders = new();

    [SerializeField][PropertyOrder(0)][FoldoutGroup("View", 0)] private bool _autoUpdateColliderComponent = true;

    [SerializeField][PropertyOrder(1)][FoldoutGroup("Create & Edit", 0)] private string _colliderName;

    [SerializeField][PropertyOrder(2)][FoldoutGroup("Save", 0)] private bool _autoSave = true;
    [SerializeField][FolderPath][PropertyOrder(2)][FoldoutGroup("Save", 1)] private string _saveFolder;

    private void Start()
    {
        var animator = GetComponentInParent<Animator>();
        Observable.EveryUpdate()
            .Select(_ => animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
            .DistinctUntilChanged()
            .Subscribe(_ => UpdateColliderComponent());
            //.AddTo(_disposables);
    }

    [Button]
    [PropertyOrder(0)][FoldoutGroup("View", 1)]
    public void UpdateColliderComponent()
    {
        if (Colliders.Count == 0) return;

        if (Colliders.Count - 1 < CurrentCollider)
        {
            Debug.LogError("No such collider! Wrong index.");
            return;
        }
        if (Colliders[CurrentCollider] == null) 
        {
            Debug.LogError("Collider asset is null!");
            return;
        }
        if (Colliders[CurrentCollider].Paths == null)
        {
            Debug.LogError($"Paths subasset is null for some reason. Fix it.");
            return;
        }

        _colliderComponent.points = new Vector2[] {};
        for (int i = 0; i < Colliders[CurrentCollider].Paths.Count; i++)
            _colliderComponent.SetPath(i, Colliders[CurrentCollider].Paths[i].Points);
    }

#if UNITY_EDITOR
    private PC2DPaths GetColliderFromComponent()
    {
        var colliderPaths = ScriptableObject.CreateInstance<PC2DPaths>();
        for (int i = 0; i < _colliderComponent.pathCount; i++)
        {
            var path = ScriptableObject.CreateInstance<PC2DPathPoints>();
            path.Points = _colliderComponent.GetPath(i);
            path.name = i.ToString();
            colliderPaths.Paths.Add(path);
        }
        return colliderPaths;
    }

    private string GetUniqueColliderName(string colliderName)
    {
        colliderName = colliderName == "" ? Colliders.Count.ToString() : colliderName;

        if (Colliders.Any(x => x.name == colliderName))
        {
            int increment = 1;
            while (Colliders.Any(x => x.name == colliderName + increment)) increment++;
            colliderName += increment;
        }

        return colliderName;
    }

    [Button][PropertyOrder(1)][FoldoutGroup("Create & Edit", 1)]
    private void AddColliderToList()
    {
        var collider = GetColliderFromComponent();
        collider.name = GetUniqueColliderName(_colliderName);
        Colliders.Add(collider);
        if (_autoSave) SaveColliderToAsset(Colliders[Colliders.Count - 1]);
    }

    [Button][PropertyOrder(2)][FoldoutGroup("Create & Edit", 2)]
    private void ChangeCurrentCollider()
    {
        var newCollider = GetColliderFromComponent();
        newCollider.name = Colliders[CurrentCollider].name;

        var assetPath = AssetDatabase.GetAssetPath(Colliders[CurrentCollider]);
        if (assetPath != "") SaveColliderToAsset(newCollider, AssetDatabase.GetAssetPath(Colliders[CurrentCollider]), true);
        Colliders[CurrentCollider] = newCollider;
    }

    [Button][PropertyOrder(2)][FoldoutGroup("Create & Edit", 3)]
    private void ChangeCurrentColliderName()
    {
        var assetPath = AssetDatabase.GetAssetPath(Colliders[CurrentCollider]);
        if (assetPath == "")
            Colliders[CurrentCollider].name = _colliderName;
        else AssetDatabase.RenameAsset(assetPath, _colliderName);
    }

    private void SaveColliderToAsset(PC2DPaths collider, string path = "", bool overwrite = false)
    {
        var assetName = collider.name;

        if (!overwrite && path != "") 
        {
            Debug.LogWarning($"Asset {assetName} already exists in {_saveFolder}. Overwrite = false. Save aborted.");
            return;
        }

        if (path == "") path = $"{_saveFolder}/{assetName}.asset";

        AssetDatabase.CreateAsset(collider, path);
        for (int i = 0; i < collider.Paths.Count; i++)
            AssetDatabase.AddObjectToAsset(collider.Paths[i], path);

        AssetDatabase.SaveAssets();
    }

    [Button][PropertyOrder(2)][FoldoutGroup("Save", 2)]
    private void SaveCurrentColliderToAsset() =>
        SaveColliderToAsset(Colliders[CurrentCollider]);

    [Button][PropertyOrder(2)][FoldoutGroup("Save", 3)]
    private void SaveAllCollidersToAssets()
    {
        for (int i = 0; i < Colliders.Count; i++)
            SaveColliderToAsset(Colliders[i]);
    }

    [OnInspectorGUI]
    private void AutoUpdateColliderComponent()
        { if (_autoUpdateColliderComponent) UpdateColliderComponent(); }
#endif
}

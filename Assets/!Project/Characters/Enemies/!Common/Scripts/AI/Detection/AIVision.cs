using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class AIVision : MonoBehaviour
{
    #region Data
    public LayerMask VisibleLayers;
    [SerializeField] private float _visionDistance = 100f;
    List<Collider2D> _overlappingColliders = new();
    ContactFilter2D _contactFilter;
    private List<GameObject> _fovObjects = new();
    private List<GameObject> _losObjects = new();
    public List<GameObject> FOVObjects => _fovObjects;
    public List<GameObject> LOSObjects => _losObjects;
    #endregion

    #region References
    [HideInInspector] public AIDetection Detection;
    private Collider2D _visionCollider;
    #endregion

    private void Awake()
    {
        _visionCollider = GetComponent<Collider2D>();
        _contactFilter = new()
        {
            layerMask = VisibleLayers,
            useLayerMask = true
        };
    }

    private void FixedUpdate()
    {
        UpdateFOVObjects();
        UpdateLOSObjects();
    }

    private void UpdateFOVObjects()
    {
        _fovObjects.Clear();

        _visionCollider.Overlap(_contactFilter, _overlappingColliders);

        foreach (Collider2D col in _overlappingColliders)
            if (!_fovObjects.Contains(col.gameObject))
                _fovObjects.Add(col.gameObject);
    }

    private void UpdateLOSObjects()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            transform.position,
            transform.right,
            _visionDistance,
            VisibleLayers
        );

        foreach (RaycastHit2D hit in hits)
            if (hit.collider != null && !_losObjects.Contains(hit.collider.gameObject))
                _losObjects.Add(hit.collider.gameObject);

        Debug.DrawRay(transform.position, transform.right * _visionDistance, Color.green, Time.fixedDeltaTime);
    }
}

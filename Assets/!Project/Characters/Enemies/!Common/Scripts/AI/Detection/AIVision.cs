using UnityEngine;
using UnityEngine.Events;

public class AIVision : MonoBehaviour
{
    #region Data
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _detectDelay;

    private float _detectTimer = 0f;
    private bool _isDetecting = false;

    [HideInInspector] public UnityEvent<GameObject> VisualDetected;
    [HideInInspector] public UnityEvent<Vector3> VisualIndirectDetected;
    [HideInInspector] public UnityEvent<GameObject> VisualLost;
    #endregion

    #region References
    [HideInInspector] public AIBehaviour AI;
    #endregion

    private void Update() 
    {
        if (_isDetecting)
            _detectTimer += Time.deltaTime;

        if (AI.TargetGameObject == null || AI.IsLosingTarget) return;
        RaycastHit2D[] hits = Physics2D.LinecastAll(AI.Agent.transform.position, AI.TargetGameObject.transform.position, _layerMask);
        foreach (var hit in hits)
        {
            if (hit.transform == null || hit.transform == AI.Agent.transform) continue;
            Player hitPlayer = hit.transform.GetComponentInParent<Player>();
            if (hitPlayer && hitPlayer.gameObject == AI.TargetGameObject && GetComponent<PolygonCollider2D>().IsTouching(hit.collider)) return;
            else
            {
                Debug.Log("Lost " + AI.TargetGameObject);
                VisualLost.Invoke(AI.TargetGameObject);
                _isDetecting = false;
                _detectTimer = 0f;
                return;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        var player = collider.GetComponentInParent<Player>();
        if (player == null || AI.TargetGameObject == player.gameObject) return;
        RaycastHit2D[] hits = Physics2D.LinecastAll(AI.Agent.transform.position, player.transform.position, _layerMask);
        foreach (var hit in hits)
        {
            if (hit.transform == null || hit.transform == AI.Agent.transform) continue;
            Player hitPlayer = hit.transform.GetComponentInParent<Player>();
            if (hitPlayer == null || hitPlayer != player) return;
            else
            {
                if (_detectTimer >= _detectDelay)
                {
                    Debug.Log("Detected " + player);
                    VisualDetected.Invoke(player.gameObject);
                    return;
                }
                else _isDetecting = true;
            }
        }
    }
}

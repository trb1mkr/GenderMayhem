using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System;
using System.Linq;

public class AIDetection : MonoBehaviour
{
    #region Data
    public AISense? TargetDetectionType; //[ReadOnly][ShowInInspector]
    [ReadOnly][ShowInInspector] public GameObject TargetGameObject;
    [ReadOnly][ShowInInspector] public Vector3 TargetPosition;

    [SerializeField] private float _newTargetPositionDeltaThreshold = 5f;
    [SerializeField] private float _targetLostDelay = 2f;
    [SerializeField] private float _targetDetectionTime = 1f;

    public Coroutine LoseCoroutine;
    public Coroutine DetectCoroutine;

    // [HideInInspector] public Action<GameObject> VisualDetected;
    // [HideInInspector] public Action<Vector3> SoundDetected;
    // [HideInInspector] public Action<GameObject> VisualLost;

    public Action TargetGameObjectDetected;
    public Action TargetGameObjectLost;
    public Action TargetPositionDetected;
    #endregion

    #region References
    [HideInInspector] public AIBehaviour AI;
    [HideInInspector] public AIVision Vision;
    [HideInInspector] public AIHearing Hearing;
    #endregion

    void Awake()
    {
        Vision = GetComponentInChildren<AIVision>();
        Hearing = GetComponentInChildren<AIHearing>();

        Vision.Detection = Hearing.Detection = this;
    }

    void Start()
    {
        Hearing.SoundDetected += (target, emitType) => OnSoundDetected(target, emitType);
    }

    void OnEnable()
    {
        Vision.enabled = true;
    }

    void OnDisable()
    {
        Vision.enabled = false;
        StopAllCoroutines();
        TargetDetectionType = null;
        TargetGameObject = null;
        TargetPosition = Vector3.zero;
        DetectCoroutine = null;
        LoseCoroutine = null;
    }

    private void FixedUpdate()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        if (DetectCoroutine != null) return;

        GameObject player = Vision.FOVObjects.FirstOrDefault(go => go.GetComponentInParent<Player>() != null);
        
        if (player == null)
        {
            if (TargetGameObject != null && LoseCoroutine == null) LoseCoroutine = StartCoroutine(LoseTargetCoroutine());
            return;
        }

        player = player.GetComponentInParent<Player>().gameObject;

        var hits = Physics2D.LinecastAll(
            AI.Agent.transform.position,
            player.transform.position,
            Vision.VisibleLayers
        );

        bool hasClearView = false;

        foreach (var hit in hits)
        {
            if (hit.transform == null || hit.transform == AI.Agent.transform) continue;

            Player hitPlayer = hit.transform.GetComponentInParent<Player>();
            if (hitPlayer && hitPlayer.gameObject == player) hasClearView = true;
            else break;
        }

        if (hasClearView)
        {
            if (LoseCoroutine != null)
            {
                StopCoroutine(LoseCoroutine);
                LoseCoroutine = null;
            }

            if (TargetGameObject != player)
                DetectCoroutine = StartCoroutine(DetectTargetCoroutine(player));
        }
        else if (TargetGameObject != null && LoseCoroutine == null)
            LoseCoroutine = StartCoroutine(LoseTargetCoroutine());
    }

    private IEnumerator DetectTargetCoroutine(GameObject player)
    {
        yield return new WaitForSeconds(_targetDetectionTime);
        OnVisualDetected(player);
        DetectCoroutine = null;
    }

    private IEnumerator LoseTargetCoroutine()
    {
        TargetGameObjectLost?.Invoke();
        yield return new WaitForSeconds(_targetLostDelay);

        if (TargetGameObject != null && !HasDirectVisionToTarget(TargetGameObject))
        {
            TargetGameObject = null;
            yield return new WaitUntil(() => AI.Navigation.NavigationMode.IsNavigating == false);
        }

        LoseCoroutine = null;
    }

    private bool HasDirectVisionToTarget(GameObject target)
    {
        var hits = Physics2D.LinecastAll(
            AI.Agent.transform.position,
            target.transform.position,
            Vision.VisibleLayers
        );

        return hits.All(hit => 
            hit.transform == AI.Agent.transform || 
            hit.transform == target.transform);
    }

    private void OnVisualDetected(GameObject target)
    {
        if (target == TargetGameObject) return;
        SenseDetected(AISense.Vision, target);
    }

    private void OnSoundDetected(GameObject target, SoundEmitType soundEmitType) =>
        SenseDetected(AISense.Hearing, target.transform.position);

    private void SenseDetected(AISense newTargetDetectionType, Vector3 position)
    {
        Debug.Log(newTargetDetectionType);
        if (TargetGameObject != null) return;
        if (TargetPosition != Vector3.zero)
            if (!IsNewTargetRelevant(newTargetDetectionType, TargetPosition, position)) return;

        TargetPosition = position;
        TargetDetectionType = newTargetDetectionType;

        TargetPositionDetected?.Invoke();
    }

    private void SenseDetected(AISense newTargetDetectionType, GameObject target)
    {
        //Debug.Log(newTargetDetectionType);
        if (TargetGameObject != null)
            if (!IsNewTargetRelevant(newTargetDetectionType, TargetGameObject.transform.position, target.transform.position)) return;

        TargetGameObject = target;
        TargetDetectionType = newTargetDetectionType;

        TargetGameObjectDetected?.Invoke();
    }

    private bool IsNewTargetRelevant(AISense newTargetDetectionType, Vector3 oldPosition, Vector3 newPosition)
    {
        //Debug.Log("New sense: " + newTargetDetectionType + ". Old sense: " + TargetDetectionType);
        if (TargetDetectionType < newTargetDetectionType) return false;
        if (TargetDetectionType == newTargetDetectionType)
        {
            var oldDistance = Vector3.Distance(transform.position, oldPosition);
            var newDistance = Vector3.Distance(transform.position, newPosition);
            if (oldDistance - newDistance < _newTargetPositionDeltaThreshold) return false;
        }
        return true;
    }
}
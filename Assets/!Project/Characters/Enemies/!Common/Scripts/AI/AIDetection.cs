using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System;
using System.Linq;

public class AIDetection : MonoBehaviour
{
    #region Data
    public AISense? TargetDetectionType; //[ReadOnly][ShowInInspector]
    public AITarget? TargetType;
    public string TargetLayer;
    [ReadOnly][ShowInInspector] public GameObject TargetGameObject;
    [ReadOnly][ShowInInspector] public Vector3 TargetPosition;

    [SerializeField] private float _newTargetPositionDeltaThreshold = 5f;

    [SerializeField] private float _playerDetectionTime = 1f;
    [SerializeField] private float _playerLostDelay = 2f;
    [SerializeField] private float _weaponDetectionTime = 0.2f;
    [SerializeField] private float _weaponLostDelay = 0f;


    public Coroutine LoseCoroutine;
    public Coroutine DetectCoroutine;

    // [HideInInspector] public Action<GameObject> VisualDetected;
    // [HideInInspector] public Action<Vector3> SoundDetected;
    // [HideInInspector] public Action<GameObject> VisualLost;

    public Action<AITarget> TargetGameObjectDetected;
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
        Hearing.SoundDetected += (target, emitType) => OnSoundDetected(target, AITarget.Sound);
        AI.Agent.ItemManager.ItemPickedUp += () => { if (AI.Agent.ItemManager.Item.gameObject == TargetGameObject) { Debug.Log("hhhh"); LoseTarget(); } };
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
        //if (AI.Agent.ItemManager.Item is Fists) DetectTarget<Weapon>("Items");
        DetectTarget<Player>("Characters");
    }

    private void DetectTarget<T>(string targetLayer) where T : MonoBehaviour
    {
        if (DetectCoroutine != null) return;

        TargetLayer = targetLayer;

        GameObject target = Vision.FOVObjects.FirstOrDefault(go => go.GetComponentInParent<T>() != null);

        if (target == null)
        {
            if (TargetGameObject != null && LoseCoroutine == null) LoseCoroutine = StartCoroutine(LoseTargetCoroutine(_playerLostDelay));
            return;
        }

        target = target.GetComponentInParent<T>().gameObject;

        var hits = Physics2D.LinecastAll(
            AI.Agent.transform.position,
            target.transform.position,
            LayerMask.GetMask(targetLayer, "Obstacles")
        );

        bool hasDirectVision = HasDirectVisionToTarget(target);

        if (hasDirectVision)
        {
            if (LoseCoroutine != null && TargetGameObject == target) //теряем цель, но снова обнаружили
            {
                StopCoroutine(LoseCoroutine);
                LoseCoroutine = null;
                TargetGameObjectDetected?.Invoke(AITarget.VisionPlayer);
            }

            if (TargetGameObject != target) //обнаружили новую цель
            {
                AITarget targetType = typeof(T) == typeof(Player) ? AITarget.VisionPlayer : AITarget.VisionWeapon;
                DetectCoroutine = StartCoroutine(DetectTargetCoroutine(target, targetType, _playerDetectionTime));
            }
        }
        else if (TargetGameObject != null && LoseCoroutine == null) //не видим цель, начинаем терять её
            LoseCoroutine = StartCoroutine(LoseTargetCoroutine(_playerLostDelay));
    }

    private IEnumerator DetectTargetCoroutine(GameObject target, AITarget targetType, float detectionTime)
    {
        yield return new WaitForSeconds(detectionTime);
        OnVisualDetected(target, targetType);
        DetectCoroutine = null;
    }

    private IEnumerator LoseTargetCoroutine(float lostDelay)
    {
        TargetGameObjectLost?.Invoke();
        yield return new WaitForSeconds(lostDelay);

        if (TargetGameObject != null && !HasDirectVisionToTarget(TargetGameObject))
        {
            TargetGameObject = null;
            yield return new WaitUntil(() => AI.Navigation.NavigationMode.IsNavigating == false);
        }

        LoseCoroutine = null;
    }

    private void LoseTarget()
    {
        TargetGameObject = null;
        LoseCoroutine = null;
        TargetGameObjectLost?.Invoke();
    }

    private bool HasDirectVisionToTarget(GameObject target)
    {
        if (target.GetComponent<Player>() && target.GetComponent<Player>().enabled == false) return false; //костыль

        var hits = Physics2D.LinecastAll(
            AI.Agent.transform.position,
            target.transform.position,
            LayerMask.GetMask(TargetLayer, "Obstacles")
        );

        foreach (var hit in hits)
        {
            if (hit.transform == AI.Agent.transform || hit.transform.IsChildOf(AI.Agent.transform)) continue;
            if (hit.transform == target.transform || hit.transform.IsChildOf(target.transform)) return true;
            else break;
        }
        return false;
    }

    private void OnVisualDetected(GameObject target, AITarget targetType)
    {
        if (target == TargetGameObject) return;
        SenseDetected(AISense.Vision, targetType, target);
    }

    private void OnSoundDetected(GameObject target, AITarget targetType) =>
        SenseDetected(AISense.Hearing, targetType, target.transform.position);

    private void SenseDetected(AISense targetDetectionType, AITarget targetType, Vector3 targetPosition)
    {
        //Debug.Log(newTargetDetectionType);
        if (TargetGameObject != null) return;
        if (TargetPosition != Vector3.zero)
            if (!IsNewTargetRelevant(targetDetectionType, targetType, TargetPosition, targetPosition)) return;

        TargetPosition = targetPosition;
        TargetDetectionType = targetDetectionType;
        TargetType = null;

        TargetPositionDetected?.Invoke();
    }

    private void SenseDetected(AISense targetDetectionType, AITarget targetType, GameObject target)
    {
        if (TargetGameObject != null)
            if (!IsNewTargetRelevant(targetDetectionType, targetType, TargetGameObject.transform.position, target.transform.position)) return;

        TargetGameObject = target;
        TargetDetectionType = targetDetectionType;
        TargetType = targetType;

        TargetGameObjectDetected?.Invoke(targetType);
    }

    private bool IsNewTargetRelevant(AISense newTargetDetectionType, AITarget newTargetType, Vector3 oldPosition, Vector3 newPosition)
    {
        //Debug.Log("New sense: " + newTargetDetectionType + ". Old sense: " + TargetDetectionType);
        if (TargetDetectionType < newTargetDetectionType) return false;
        if (TargetType < newTargetType) return false;
        if (TargetDetectionType == newTargetDetectionType)
        {
            var oldDistance = Vector3.Distance(transform.position, oldPosition);
            var newDistance = Vector3.Distance(transform.position, newPosition);
            if (oldDistance - newDistance < _newTargetPositionDeltaThreshold) return false;
        }
        return true;
    }
}
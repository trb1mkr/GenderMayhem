using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour, IAudioSourceListener
{
    #region Data
    [ReadOnly][ShowInInspector] public AINavigationMode NavigationMode;

    public AISenses? TargetDetectionType; //[ReadOnly][ShowInInspector]
    [ReadOnly][ShowInInspector] public GameObject TargetGameObject;
    [ReadOnly][ShowInInspector] public Vector3 TargetPosition;
    [SerializeField] private float _targetLostDelay;
    [ReadOnly][ShowInInspector] public bool IsLosingTarget;
    [SerializeField] private float _newTargetPositionDeltaThreshold = 5f;
    private Coroutine _targetLost;

    #endregion

    #region References
    [HideInInspector] public Enemy Agent;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public AIMovement Movement;
    [HideInInspector] public AIPatrol Patrol;
    [HideInInspector] public AIPursuit Pursuit;
    [HideInInspector] public AISearch Search;
    [HideInInspector] public AICheck Check;
    [HideInInspector] public AIRotateToDirection Rotation;
    [HideInInspector] public AIVision Vision;
    #endregion

    void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Movement = GetComponent<AIMovement>();
        Patrol = GetComponent<AIPatrol>();
        Pursuit = GetComponent<AIPursuit>();
        Search = GetComponent<AISearch>();
        Check = GetComponent<AICheck>();
        Rotation = GetComponent<AIRotateToDirection>();
        Vision = GetComponentInChildren<AIVision>();

        Movement.AI = Patrol.AI = Pursuit.AI = Search.AI = Check.AI = Rotation.AI = Vision.AI = this;
    }

    private void Start()
    {
        AddListeners();
        SetNavigationMode(Patrol);
    }

    private void AddListeners()
    {
        Vision.VisualDetected.AddListener(OnVisualDetected);
        Vision.VisualLost.AddListener(OnVisualLost);

        Search.Ended += () => SetNavigationMode(Patrol);
        Check.Ended += () => SetNavigationMode(Search);
    }

    #region OnTargetDetected
    private void OnVisualDetected(GameObject target)
    {
        if (target == TargetGameObject) return;
        if (_targetLost != null) StopCoroutine(_targetLost);
        IsLosingTarget = false;
        SenseDetected(AISenses.Vision, Pursuit, target);
    }

    public void OnSoundDetected(GameObject target, SoundEmitType soundEmitType) =>
        SenseDetected(AISenses.Hearing, Check, target.transform.position);

    private void SenseDetected(AISenses newTargetDetectionType, AINavigationMode navigationMode, Vector3 position)
    {
        //Debug.Log(newTargetDetectionType);
        if (TargetGameObject != null) return;
        if (TargetPosition != Vector3.zero)
            if (!IsNewTargetRelevant(newTargetDetectionType, TargetPosition, position)) return;

        TargetPosition = position;
        TargetDetectionType = newTargetDetectionType;

        SetNavigationMode(navigationMode);
    }

    private void SenseDetected(AISenses newTargetDetectionType, AINavigationMode navigationMode, GameObject target)
    {
        //Debug.Log(newTargetDetectionType);
        if (TargetGameObject != null)
            if (!IsNewTargetRelevant(newTargetDetectionType, TargetGameObject.transform.position, target.transform.position)) return;

        TargetGameObject = target;
        TargetDetectionType = newTargetDetectionType;

        SetNavigationMode(navigationMode);
    }

    private bool IsNewTargetRelevant(AISenses newTargetDetectionType, Vector3 oldPosition, Vector3 newPosition)
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

    private void SetNavigationMode(AINavigationMode navigationMode)
    {
        if (NavigationMode && NavigationMode.IsNavigating) NavigationMode.StopNavigation();
        NavigationMode = navigationMode;
        NavigationMode.StartNavigation();
    }
    #endregion

    #region OnTargetLost
    private void OnVisualLost(GameObject target)
    {
        if (TargetGameObject != target) return;
        if (TargetGameObject == target && IsLosingTarget) return;
        _targetLost = StartCoroutine(OnTargetLost(_targetLostDelay));
    }

    public IEnumerator OnTargetLost(float delay)
    {
        IsLosingTarget = true;
        yield return new WaitForSeconds(delay);
        TargetGameObject = null;
        TargetDetectionType = null;
        yield return new WaitUntil(() => NavigationMode.IsNavigating == false);
        IsLosingTarget = false;
        SetNavigationMode(Search);
    }
    #endregion

    private void OnDrawGizmos() => DrawAgentPath(NavMeshAgent);

    public static void DrawAgentPath(NavMeshAgent agent)
    {
        if (Application.isPlaying && agent != null)
        {
            if (agent.hasPath)
            {
                var corners = agent.path.corners;
                if (corners.Length < 2) { return; }
                int i = 0;
                for (; i < corners.Length - 1; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(corners[i], corners[i + 1]);
                    Gizmos.DrawSphere(agent.path.corners[i + 1], 0.03f);
                    Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
                }
                Gizmos.color = Color.red;
                Gizmos.DrawLine(corners[0], corners[1]);
                Gizmos.DrawSphere(agent.path.corners[1], 0.03f);
                Gizmos.DrawLine(agent.path.corners[0], agent.path.corners[1]);
            }
        }
    }
}

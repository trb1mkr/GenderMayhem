using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour: MonoBehaviour
{
    #region References
    [HideInInspector] public Enemy Agent;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public AIMovement Movement;
    [HideInInspector] public AINavigation Navigation;
    [HideInInspector] public AIDetection Detection;
    [HideInInspector] public AIRotation Rotation;
    [HideInInspector] public AIWeaponController WeaponController;
    #endregion

    void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Detection = GetComponent<AIDetection>();
        Navigation = GetComponent<AINavigation>();
        Movement = GetComponent<AIMovement>();
        Rotation = GetComponent<AIRotation>();
        WeaponController = GetComponent<AIWeaponController>();

        Movement.AI = Rotation.AI = Detection.AI = Navigation.AI = WeaponController.AI = this;

        NavMesh.pathfindingIterationsPerFrame = 5000;
        NavMesh.avoidancePredictionTime = 4f; 
    }

    void OnEnable()
    {
        Navigation.enabled = true;
        Detection.enabled = true;
        Rotation.enabled = true;
    }

    void OnDisable()
    {
        Navigation.enabled = false;
        Detection.enabled = false;
        Rotation.enabled = false;
    }

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

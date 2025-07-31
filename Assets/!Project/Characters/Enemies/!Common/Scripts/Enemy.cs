using System.Collections.Generic;
using UnityEngine.Splines;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public NavMeshAgent Agent;
    public List<SplineContainer> PatrolPaths;

    new void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos() => DrawAgentPath(Agent);

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

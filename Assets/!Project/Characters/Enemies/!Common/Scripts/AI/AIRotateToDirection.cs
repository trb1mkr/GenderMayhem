using UnityEngine;

public class AIRotateToDirection : MonoBehaviour
{
    [HideInInspector] public AIBehaviour AI;

    void Update()
    {
        RotateToDirection();
    }
    
    private void RotateToDirection()
    {
        if (AI.NavMeshAgent.hasPath && AI.NavMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            Vector2 direction = AI.NavMeshAgent.velocity.normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

            AI.NavMeshAgent.transform.rotation = Quaternion.Lerp(
                AI.NavMeshAgent.transform.rotation,
                targetRotation,
                AI.NavMeshAgent.angularSpeed * Time.deltaTime
            );
        }
    }
}

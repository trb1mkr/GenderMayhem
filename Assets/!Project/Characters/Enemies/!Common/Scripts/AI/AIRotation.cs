using UnityEngine;

public class AIRotation : MonoBehaviour
{
    [HideInInspector] public AIBehaviour AI;

    void Update()
    {
        if (AI.Pursuit.IsNavigating && !AI.IsLosingTarget && AI.TargetGameObject != null) RotateToTarget(AI.TargetGameObject);
        else RotateToMoveDirection();
    }

    private void RotateToMoveDirection()
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

    private void RotateToTarget(GameObject target)
    {
        Vector2 direction = target.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, AI.NavMeshAgent.angularSpeed * Time.deltaTime);
    }
}

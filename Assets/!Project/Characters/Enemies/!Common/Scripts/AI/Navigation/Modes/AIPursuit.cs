public class AIPursuit : AINavigationMode
{
    private bool _hasPath = false;
    
    private void Update()
    {
        if (!IsNavigating) return;
        if (AI.TargetGameObject != null)
        {
            // Обновляем путь к цели, если она существует
            AI.NavMeshAgent.SetDestination(AI.TargetGameObject.transform.position);
            _hasPath = true;
        }
        else if (_hasPath)
        {
            // Если цели нет, но путь еще есть - проверяем, дошли ли до конца
            if (!AI.NavMeshAgent.pathPending && AI.NavMeshAgent.remainingDistance <= AI.NavMeshAgent.stoppingDistance)
            {
                if (!AI.NavMeshAgent.hasPath || AI.NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    // Дошли до конца пути - останавливаемся
                    EndNavigation();
                    _hasPath = false;
                }
            }
        }
    }
}


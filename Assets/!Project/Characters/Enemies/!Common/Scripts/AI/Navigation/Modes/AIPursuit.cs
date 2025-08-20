public class AIPursuit : AINavigationMode
{
    private bool _hasPath = false;

    void Start()
    {
        Navigation.AI.Agent.ItemManager.ItemChanged += () => { if (Navigation.AI.Agent.ItemManager.Item is Weapon weapon) StoppingDistance = weapon.AttackDistance; };
    }

    private void Update()
    {
        if (!IsNavigating) return;
        if (Navigation.AI.Detection.TargetGameObject != null)
        {
            // Обновляем путь к цели, если она существует
            Navigation.AI.NavMeshAgent.SetDestination(Navigation.AI.Detection.TargetGameObject.transform.position);
            _hasPath = true;
        }
        else if (_hasPath)
        {
            // Если цели нет, но путь еще есть - проверяем, дошли ли до конца
            if (!Navigation.AI.NavMeshAgent.pathPending && Navigation.AI.NavMeshAgent.remainingDistance <= Navigation.AI.NavMeshAgent.stoppingDistance)
            {
                if (!Navigation.AI.NavMeshAgent.hasPath || Navigation.AI.NavMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    // Дошли до конца пути - останавливаемся
                    EndNavigation();
                    _hasPath = false;
                }
            }
        }
    }
}


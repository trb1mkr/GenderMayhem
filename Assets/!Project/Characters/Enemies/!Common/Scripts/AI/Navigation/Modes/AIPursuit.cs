using UnityEngine;
using R3;

public class AIPursuit : AINavigationMode
{
    [SerializeField] private SerializableReactiveProperty<float> _dynamicStoppingDistance;
    private bool _hasPath = false;

    void Start()
    {
        //Navigation.AI.Agent.ItemManager.ItemChanged += () => { if (Navigation.AI.Agent.ItemManager.Item is Weapon weapon) _dynamicStoppingDistance.Value = weapon.AttackDistance; };
        //Navigation.AI.Detection.TargetGameObjectDetected
        Started += () => { if (Navigation.AI.Agent.ItemManager.Item is Weapon weapon) _dynamicStoppingDistance.Value = weapon.AttackDistance; };
        Navigation.AI.Detection.TargetGameObjectLost += () => _dynamicStoppingDistance.Value = StoppingDistance;

        //is navigating происходит позже, чем обнаруживается цель
        _dynamicStoppingDistance.Subscribe(stoppingDistance => { if (IsNavigating) { Navigation.AI.NavMeshAgent.stoppingDistance = stoppingDistance; } });
        _dynamicStoppingDistance.Value = StoppingDistance;
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


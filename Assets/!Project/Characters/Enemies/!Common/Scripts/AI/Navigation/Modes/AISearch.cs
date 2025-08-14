using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public partial class AISearch : AINavigationMode
{
    [HideInInspector] public Enemy Agent;
    [SerializeField] private Vector3 _searchPosition;
    [SerializeField] private int _searchPointsCount = 3;
    [SerializeField] private float _searchWaitTime = 0.1f;
    [SerializeField] private float _radiusFromSearchPosition = 10f;

    public override void StartNavigation()
    {
        base.StartNavigation();
        StartCoroutine(Search());
    }

    private IEnumerator Search()
    {
        _searchPosition = transform.position; // Запоминаем позицию, где потеряли игрока

        for (int i = 0; i < _searchPointsCount; i++)
        {
            // Генерируем случайную точку в радиусе поиска
            Vector3 randomPoint = _searchPosition + Random.insideUnitSphere * _radiusFromSearchPosition;
            randomPoint.z = 0; // Для 2D игры

            // Ищем ближайшую точку на NavMesh
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                AI.NavMeshAgent.SetDestination(hit.position);

                // Ждем пока дойдем до точки
                yield return new WaitUntil(() => !AI.NavMeshAgent.pathPending && AI.NavMeshAgent.remainingDistance <= AI.NavMeshAgent.stoppingDistance);

                // Стоим на точке заданное время
                yield return new WaitForSeconds(_searchWaitTime);
            }
        }

        // Завершаем поиск
        EndNavigation();
    }
}


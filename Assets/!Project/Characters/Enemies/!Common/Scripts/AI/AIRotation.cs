using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;

public class AIRotation : MonoBehaviour
{
    #region Data
    [SerializeField] private float _aimingThreshold = 5f;
    private Vector3 _lastPredictedPosition;
    [ShowInInspector, ReadOnly] private bool _isAimed;
    public Vector3 LastPredictedPosition => _lastPredictedPosition;
    public bool IsAimed => _isAimed;

    public Action LookedAround;
    private Coroutine _rotationCoroutine;
    #endregion

    #region References
    [HideInInspector] public AIBehaviour AI;
    #endregion

    void Start()
    {
        AI.Navigation.Pursuit.Started += () => { if (AI.Detection.LoseCoroutine == null && AI.Agent.ItemManager.Item is Melee) StartRotation(RotateToAimAt(AI.Detection.TargetGameObject)); };
        AI.Navigation.Pursuit.Started += () => { if (AI.Detection.LoseCoroutine == null && AI.Agent.ItemManager.Item is Gun) StartRotation(RotateToAimAheadOf(AI.Detection.TargetGameObject)); };
        AI.Navigation.Search.Started += () => StartRotation(RotateToMoveDirection());
        AI.Navigation.Patrol.Started += () => StartRotation(RotateToMoveDirection());
        AI.Navigation.MoveTo.Started += () => StartRotation(RotateToMoveDirection());
        AI.Detection.TargetGameObjectLost += () => StartRotation(RotateToMoveDirection());
        AI.Agent.Health.StoodUp += () => StartRotation(RotateAround(360f));

        StartRotation(RotateToMoveDirection());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private void StartRotation(IEnumerator coroutine)
    {
        StopAllCoroutines();
        _rotationCoroutine = StartCoroutine(coroutine);
    }

    private IEnumerator RotateToMoveDirection()
    {
        while (true)
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
            yield return null;
        }
    }

    private IEnumerator RotateToAimAt(GameObject target)
    {
        while (target != null)
        {
            Vector2 direction = target.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                AI.NavMeshAgent.angularSpeed * Time.deltaTime
            );

            Vector3 aimDirection = (target.transform.position - transform.position).normalized;
            Debug.Log(Vector3.Angle(transform.right, aimDirection));
            _isAimed = Vector3.Angle(transform.right, aimDirection) <= _aimingThreshold;

            yield return null;
        }
    }

    private IEnumerator RotateToAimAheadOf(GameObject target)
    {
        while (true)
        {
            // Предсказание позиции
            float distance = Vector3.Distance(transform.position, target.transform.position);
            float travelTime = distance / ((Gun)AI.Agent.ItemManager.Item).GunUtilities.Bullet.GetComponent<Bullet>().Force;
            _lastPredictedPosition = target.transform.position + (Vector3)target.GetComponent<Rigidbody2D>().linearVelocity * travelTime;

            // Поворот к цели
            Vector2 aimDirection = ((Vector2)_lastPredictedPosition - (Vector2)transform.position).normalized;

            float targetAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, AI.NavMeshAgent.angularSpeed * 50f * Time.deltaTime);

            // Проверка наведения
            _isAimed = Vector2.Angle(transform.right, aimDirection) <= _aimingThreshold;
            //Debug.Log(Vector2.Angle(transform.right, aimDirection));

            yield return null;
        }
    }

    public IEnumerator RotateAround(float targetAngle, bool clockwise = true)
    {
        float anglePassed = 0f;
        float directionMultiplier = clockwise ? 1f : -1f;
        Quaternion startRotation = transform.rotation;
        float absTargetAngle = Mathf.Abs(targetAngle);

        while (anglePassed < absTargetAngle)
        {
            float rotationStep = AI.NavMeshAgent.angularSpeed * 50f * Time.deltaTime; //50f
            anglePassed = Mathf.Min(anglePassed + rotationStep, absTargetAngle);

            float currentAngle = anglePassed * directionMultiplier;
            transform.rotation = startRotation * Quaternion.Euler(0f, 0f, currentAngle);
            yield return null;
        }

        // Финализируем поворот с точным значением угла
        transform.rotation = startRotation * Quaternion.Euler(0f, 0f, targetAngle * directionMultiplier);

        LookedAround?.Invoke();
    }
}

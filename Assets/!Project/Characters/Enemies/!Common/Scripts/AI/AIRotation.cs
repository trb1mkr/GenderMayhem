using UnityEngine;
using System.Collections;
using System;

public class AIRotation : MonoBehaviour
{
    #region Data
    public Action LookedAround;
    private Coroutine _rotationCoroutine;
    #endregion

    #region References
    [HideInInspector] public AIBehaviour AI;
    #endregion

    void Start()
    {
        AI.Agent.Health.StoodUp += () => StartRotation(RotateAround(360f));
        AI.Navigation.NavigationModeChanged += NavigationMode => { if (NavigationMode && NavigationMode == AI.Navigation.Pursuit && AI.Detection.LoseCoroutine == null) StartRotation(RotateToTarget(AI.Detection.TargetGameObject)); };
        AI.Navigation.NavigationModeChanged += NavigationMode => { if (NavigationMode && NavigationMode != AI.Navigation.Pursuit) StartRotation(RotateToMoveDirection()); };
        AI.Detection.TargetGameObjectLost += () => StartRotation(RotateToMoveDirection());

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

    private IEnumerator RotateToTarget(GameObject target)
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

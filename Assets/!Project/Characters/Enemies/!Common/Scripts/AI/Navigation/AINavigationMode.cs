using UnityEngine;
using System;
using Sirenix.OdinInspector;

public abstract class AINavigationMode : MonoBehaviour
{
    [ReadOnly] public bool IsNavigating;
    [SerializeField] protected float StoppingDistance = 0.1f;
    public Action Started;
    public Action Stopped;
    public Action Ended;

    [HideInInspector] public AIBehaviour AI;

    public virtual void StartNavigation()
    {
        Debug.Log(GetType().Name + " started");
        IsNavigating = true;
        AI.NavMeshAgent.stoppingDistance = StoppingDistance;
        Started?.Invoke();
    }

    public virtual void StopNavigation()
    {
        Debug.Log(GetType().Name + " stopped");
        IsNavigating = false;
        StopAllCoroutines();
        AI.NavMeshAgent.ResetPath();
        Stopped?.Invoke();
    }

    protected virtual void EndNavigation()
    {
        Debug.Log(GetType().Name + " ended");
        IsNavigating = false;
        AI.NavMeshAgent.ResetPath();
        Ended?.Invoke();
    }

    public virtual void TerminateNavigation()
    {
        Debug.Log(GetType().Name + " terminated");
        IsNavigating = false;
        AI.NavMeshAgent.ResetPath();
    }
}
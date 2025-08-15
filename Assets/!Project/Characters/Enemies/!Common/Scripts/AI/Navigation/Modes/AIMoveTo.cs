public class AIMoveTo : AINavigationMode
{
    public override void StartNavigation()
    {
        base.StartNavigation();
        AI.NavMeshAgent.SetDestination(AI.TargetPosition);
    }

    void Update()
    {
        if (!IsNavigating) return;
        if (!AI.NavMeshAgent.pathPending && AI.NavMeshAgent.remainingDistance < StoppingDistance)
        {
            AI.TargetPosition = UnityEngine.Vector3.zero;
            EndNavigation();
        }
    }
}

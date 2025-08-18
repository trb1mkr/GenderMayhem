public class AIMoveTo : AINavigationMode
{
    public override void StartNavigation()
    {
        base.StartNavigation();
        Navigation.AI.NavMeshAgent.SetDestination(Navigation.AI.Detection.TargetPosition);
    }

    void Update()
    {
        if (!IsNavigating) return;
        if (!Navigation.AI.NavMeshAgent.pathPending && Navigation.AI.NavMeshAgent.remainingDistance < StoppingDistance)
        {
            Navigation.AI.Detection.TargetPosition = UnityEngine.Vector3.zero;
            EndNavigation();
        }
    }
}

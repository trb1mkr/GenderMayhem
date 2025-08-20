using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class AINavigation : MonoBehaviour
{
    #region References
    [HideInInspector] public AIBehaviour AI;
    [ReadOnly][ShowInInspector] public AINavigationMode NavigationMode;
    [HideInInspector] public AIPatrol Patrol;
    [HideInInspector] public AIPursuit Pursuit;
    [HideInInspector] public AISearch Search;
    [HideInInspector] public AIMoveTo MoveTo;
    public Action<AINavigationMode?> NavigationModeChanged;
    #endregion

    void Awake()
    {
        Patrol = GetComponent<AIPatrol>();
        Pursuit = GetComponent<AIPursuit>();
        Search = GetComponent<AISearch>();
        MoveTo = GetComponent<AIMoveTo>();

        Patrol.Navigation = Pursuit.Navigation = Search.Navigation = MoveTo.Navigation = this;
    }

    void Start()
    {
        AddListeners();
        SetNavigationMode(Patrol);
    }

    void OnDisable()
    {
        if (NavigationMode) NavigationMode.TerminateNavigation();
        NavigationMode = null;
    }

     private void AddListeners()
    {
        AI.Rotation.LookedAround += () => SetNavigationMode(Search);
        AI.Detection.TargetGameObjectDetected += () => SetNavigationMode(Pursuit);
        AI.Detection.TargetPositionDetected += () => SetNavigationMode(MoveTo);
        //AI.Detection.TargetGameObjectLost += () => SetNavigationMode(Search);

        Pursuit.Ended += () => SetNavigationMode(Search);
        Search.Ended += () => SetNavigationMode(Patrol);
        MoveTo.Ended += () => SetNavigationMode(Search);
    }

    private void RemoveListeners()
    {
        AI.Detection.TargetGameObjectDetected -= () => SetNavigationMode(Pursuit);
        AI.Detection.TargetPositionDetected -= () => SetNavigationMode(MoveTo);
        AI.Detection.TargetGameObjectLost -= () => SetNavigationMode(Search);

        Search.Ended -= () => SetNavigationMode(Patrol);
        MoveTo.Ended -= () => SetNavigationMode(Search);
    }

    public void SetNavigationMode(AINavigationMode navigationMode)
    {
        if (NavigationMode && NavigationMode.IsNavigating) NavigationMode.StopNavigation();
        NavigationMode = navigationMode;
        NavigationMode.StartNavigation();
        NavigationModeChanged?.Invoke(navigationMode);
    }
}

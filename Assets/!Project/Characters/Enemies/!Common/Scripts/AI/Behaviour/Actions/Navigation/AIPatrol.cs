using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol Splines", story: "[AI] patrols", category: "Action/Navigation", id: "1d9173e3676d2ebad7e89b0839d764bf")]
public partial class AIPatrol : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> AI;
    [SerializeReference] public BlackboardVariable<float> PatrolSpeed = new(0f);
    [SerializeReference] public BlackboardVariable<float> WaypointWaitTime = new(0f);
    [SerializeReference] public BlackboardVariable<float> StoppingDistanceThreshold = new(0.2f);

    private NavMeshAgent _agent;
    private Vector3 _currentTarget;
    private float _waypointWaitTimer;
    private int _currentPatrolSplineContainer = 0;
    private int _currentPatrolSpline = 0;
    private int _currentPatrolWaypoint = -1;
    private bool _isWaiting;
    private bool _isBacktracking;

    protected override Status OnStart()
    {
        if (AI.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }

        if (AI.Value.PatrolPaths.Count == 0 || AI.Value.PatrolPaths == null)
        {
            LogFailure("No waypoints to patrol.");
            return Status.Failure;
        }

        Initialize();

        _isWaiting = false;
        _waypointWaitTimer = 0.0f;

        SetDestinationToNextWaypoint();
        return Status.Running;
    }

    private void Initialize()
    {
        _agent = AI.Value.GetComponentInChildren<NavMeshAgent>();
        if (_agent != null)
        {
            if (_agent.isOnNavMesh)
                _agent.ResetPath();

            _agent.speed = PatrolSpeed.Value;
            _agent.stoppingDistance = StoppingDistanceThreshold;
        }
    }

    protected override Status OnUpdate()
    {
        if (AI.Value == null || AI.Value.PatrolPaths == null)
            return Status.Failure;

        if (_isWaiting)
        {
            if (_waypointWaitTimer > 0.0f)
                _waypointWaitTimer -= Time.deltaTime;
            else
            {
                _waypointWaitTimer = 0f;
                _isWaiting = false;
                SetDestinationToNextWaypoint();
            }
        }
        else
        {
            float distance = _agent.remainingDistance;
            bool destinationReached = distance <= StoppingDistanceThreshold;

            // Check if we've reached the waypoint (ensuring NavMeshAgent has completed path calculation if available)
            if (destinationReached && (_agent == null || !_agent.pathPending))
            {
                _waypointWaitTimer = WaypointWaitTime.Value;
                _isWaiting = true;

                return Status.Running;
            }
        }

        RotateToDirection();

        return Status.Running;
    }

    private void RotateToDirection()
    {
        if (_agent.hasPath && _agent.velocity.sqrMagnitude > 0.01f)
        {
            Vector2 direction = _agent.velocity.normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

            _agent.transform.rotation = Quaternion.Lerp(
                _agent.transform.rotation,
                targetRotation,
                _agent.angularSpeed * Time.deltaTime
            );
        }
    }

    protected override void OnEnd()
    {
        if (_agent != null)
        {
            if (_agent.isOnNavMesh)
                _agent.ResetPath();
        }
    }

    private void SetDestinationToNextWaypoint()
    {
        bool isPatrolWaypointLast = _currentPatrolWaypoint + 1 == AI.Value.PatrolPaths[_currentPatrolSplineContainer].Splines[_currentPatrolSpline].Count;
        bool isPatrolSplineLast = _currentPatrolSpline + 1 == AI.Value.PatrolPaths[_currentPatrolSplineContainer].Splines.Count;
        bool isPatrolSplineContainerLast = _currentPatrolSplineContainer + 1 == AI.Value.PatrolPaths.Count;

        if (!isPatrolWaypointLast && !_isBacktracking)
            _currentPatrolWaypoint++;
        if (_currentPatrolWaypoint != 0 && _isBacktracking)
            _currentPatrolWaypoint--;
        
        if (!isPatrolSplineLast && isPatrolWaypointLast && !_isBacktracking)
            _currentPatrolSpline++;
        if (_currentPatrolSpline != 0 && _currentPatrolWaypoint == 0 && _isBacktracking)
            _currentPatrolSpline--;

        if (!isPatrolSplineContainerLast && isPatrolSplineLast && isPatrolWaypointLast && !_isBacktracking)
            _currentPatrolSplineContainer++;
        if (_currentPatrolSplineContainer != 0 && _currentPatrolSpline == 0 && _currentPatrolWaypoint == 0 && _isBacktracking)
            _currentPatrolSplineContainer--;

        if (isPatrolSplineContainerLast && isPatrolSplineLast && isPatrolWaypointLast)
        {
            _isBacktracking = !AI.Value.PatrolPaths[_currentPatrolSplineContainer].Splines[_currentPatrolSpline].Closed;
            if (_isBacktracking == false)
                _currentPatrolSplineContainer = _currentPatrolSpline = _currentPatrolWaypoint = 0;
        }

        if (_currentPatrolWaypoint == 0 && _currentPatrolSpline == 0 && _currentPatrolSplineContainer == 0 && _isBacktracking)
            _isBacktracking = false;

        var _currentKnot = AI.Value.PatrolPaths[_currentPatrolSplineContainer].Spline[_currentPatrolWaypoint];
        var _currentKnotPosition = new Vector3(_currentKnot.Position.x, _currentKnot.Position.y, _currentKnot.Position.z);
        _currentTarget = AI.Value.PatrolPaths[_currentPatrolSplineContainer].transform.position + _currentKnotPosition;
        _agent.SetDestination(_currentTarget);
    }
}


using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines;
using Sirenix.OdinInspector;

public class AIPatrol : AINavigationMode
{
    [SerializeField] private float _patrolWaitTime = 0f;
    [SerializeField] private bool _restartFromBeginning;
    [SerializeField] private List<SplineContainer> _patrolPaths;

    private Vector3 _currentTarget;
    private float _waypointWaitTimer;
    [ShowInInspector][ReadOnly] private int _currentPatrolSplineContainer = 0;
    [ShowInInspector][ReadOnly] private int _currentPatrolSpline = 0;
    [ShowInInspector][ReadOnly] private int _currentPatrolWaypoint = -1;
    private bool _isWaiting;
    [ShowInInspector][ReadOnly] private bool _isBacktracking;

    public override void StartNavigation()
    {
        if (_patrolPaths.Count == 0 || _patrolPaths == null)
        {
            Debug.LogError("No waypoints to patrol.");
            return;
        }

        _isWaiting = false;
        _waypointWaitTimer = 0.0f;
        base.StartNavigation();

        SetDestinationToNextWaypoint();
    }

    private void Update()
    {
        if (AI == null || _patrolPaths == null || IsNavigating == false) return;

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
            float distance = AI.NavMeshAgent.remainingDistance;
            bool destinationReached = distance <= StoppingDistance;

            // Check if we've reached the waypoint (ensuring NavMeshAgent has completed path calculation if available)
            if (destinationReached && (AI.NavMeshAgent == null || !AI.NavMeshAgent.pathPending))
            {
                _waypointWaitTimer = _patrolWaitTime;
                _isWaiting = true;
            }
        }
    }

    private void SetDestinationToNextWaypoint()
    {
        bool isPatrolWaypointLast = _currentPatrolWaypoint + 1 == _patrolPaths[_currentPatrolSplineContainer].Splines[_currentPatrolSpline].Count;
        bool isPatrolSplineLast = _currentPatrolSpline + 1 == _patrolPaths[_currentPatrolSplineContainer].Splines.Count;
        bool isPatrolSplineContainerLast = _currentPatrolSplineContainer + 1 == _patrolPaths.Count;

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
            _isBacktracking = !_patrolPaths[_currentPatrolSplineContainer].Splines[_currentPatrolSpline].Closed;
            if (_isBacktracking == false)
                _currentPatrolSplineContainer = _currentPatrolSpline = _currentPatrolWaypoint = 0;
        }

        if (_currentPatrolWaypoint == 0 && _currentPatrolSpline == 0 && _currentPatrolSplineContainer == 0 && _isBacktracking)
            _isBacktracking = false;

        var _currentKnot = _patrolPaths[_currentPatrolSplineContainer].Spline[_currentPatrolWaypoint];
        var _currentKnotPosition = new Vector3(_currentKnot.Position.x, _currentKnot.Position.y, _currentKnot.Position.z);
        _currentTarget = _patrolPaths[_currentPatrolSplineContainer].transform.position + _currentKnotPosition;
        AI.NavMeshAgent.SetDestination(_currentTarget);
    }

    public override void StopNavigation()
    {
        base.StopNavigation();
        if (_restartFromBeginning) SetOriginalValues();
    }

    public override void EndNavigation()
    {
        base.EndNavigation();
        if (_restartFromBeginning) SetOriginalValues();
    }

    private void SetOriginalValues()
    {
        _currentPatrolSplineContainer = 0;
        _currentPatrolSpline = 0;
        _currentPatrolWaypoint = -1;
        _isBacktracking = false;
    }
}

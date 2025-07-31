using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Splines;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "[Agent] patrols", category: "Action", id: "1d9173e3676d2ebad7e89b0839d764bf")]
public partial class PatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Agent;

    //[SerializeReference] public BlackboardVariable<GameObject> Agent;
    //[SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;
    [SerializeReference] public BlackboardVariable<float> Speed = new(3f);
    [SerializeReference] public BlackboardVariable<float> WaypointWaitTime = new(1.0f);
    [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(0.2f);
    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new("SpeedMagnitude");
    [Tooltip("Should patrol restart from the latest point?")]
    //[SerializeReference] public BlackboardVariable<bool> PreserveLatestPatrolPoint = new (false);

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    [CreateProperty] private Vector3 _currentTarget;
    [CreateProperty] private float _originalStoppingDistance = -1f;
    [CreateProperty] private float _originalSpeed = -1f;
    [CreateProperty] private float _waypointWaitTimer;
    private float _currentSpeed;
    [CreateProperty] private int _currentPatrolPoint = 0;
    [CreateProperty] private int _currentPatrolPath = 0;
    [CreateProperty] private bool _waiting;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }

        if (Agent.Value.PatrolPaths.Count == 0 || Agent.Value.PatrolPaths == null)
        {
            LogFailure("No waypoints to patrol assigned.");
            return Status.Failure;
        }

        Initialize();

        _waiting = false;
        _waypointWaitTimer = 0.0f;

        MoveToNextWaypoint();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Agent.Value.PatrolPaths == null)
        {
            return Status.Failure;
        }

        if (_waiting)
        {
            if (_waypointWaitTimer > 0.0f)
            {
                _waypointWaitTimer -= Time.deltaTime;
            }
            else
            {
                _waypointWaitTimer = 0f;
                _waiting = false;
                MoveToNextWaypoint();
            }
        }
        else
        {
            float distance = GetDistanceToWaypoint();
            bool destinationReached = distance <= DistanceThreshold;

            // Check if we've reached the waypoint (ensuring NavMeshAgent has completed path calculation if available)
            if (destinationReached && (_navMeshAgent == null || !_navMeshAgent.pathPending))
            {
                _waypointWaitTimer = WaypointWaitTime.Value;
                _waiting = true;
                _currentSpeed = 0;

                return Status.Running;
            }
        }

        //UpdateAnimatorSpeed();
        RotateToPath();
        return Status.Running;
    }

    private void RotateToPath()
    {
        if (_navMeshAgent.hasPath && _navMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            // Получаем направление движения
            Vector2 direction = _navMeshAgent.velocity.normalized;
            
            // Вычисляем угол поворота в радианах, затем конвертируем в градусы
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Создаем целевой поворот (в 2D мы вращаем по оси Z)
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            
            // Плавно интерполируем текущий поворот к целевому
            _navMeshAgent.transform.rotation = Quaternion.Lerp(
                _navMeshAgent.transform.rotation,
                targetRotation,
                _navMeshAgent.angularSpeed * Time.deltaTime
            );
        }
    }

    protected override void OnEnd()
    {
        //UpdateAnimatorSpeed(0f);

        if (_navMeshAgent != null)
        {
            if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.ResetPath();
            }
            _navMeshAgent.speed = _originalSpeed;
            _navMeshAgent.stoppingDistance = _originalStoppingDistance;
        }
    }

    // protected override void OnDeserialize()
    // {
    //     // If using a navigation mesh, we need to reset default value before Initialize.
    //     _navMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
    //     if (_navMeshAgent != null)
    //     {
    //         if (_originalSpeed >= 0f)
    //             _navMeshAgent.speed = _originalSpeed;
    //         if (_originalStoppingDistance >= 0f)
    //             _navMeshAgent.stoppingDistance = _originalStoppingDistance;

    //         _navMeshAgent.Warp(Agent.Value.transform.position);
    //     }

    //     //int patrolPoint = _currentPatrolPoint - 1;
    //     Initialize();
    //     // During deserialization, bypass PreserveLatestPatrolPoint.
    //     //_currentPatrolPoint = patrolPoint;
    // }

    private void Initialize()
    {
        _animator = Agent.Value.GetComponentInChildren<Animator>();
        _navMeshAgent = Agent.Value.GetComponentInChildren<NavMeshAgent>();
        if (_navMeshAgent != null)
        {
            if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.ResetPath();
            }

            _originalSpeed = _navMeshAgent.speed;
            _navMeshAgent.speed = Speed.Value;
            _originalStoppingDistance = _navMeshAgent.stoppingDistance;
            _navMeshAgent.stoppingDistance = DistanceThreshold;
        }

        //_currentPatrolPoint = PreserveLatestPatrolPoint.Value ? _currentPatrolPoint - 1 : -1;

        //UpdateAnimatorSpeed(0f);
    }

    private float GetDistanceToWaypoint()
    {
        if (_navMeshAgent != null)
        {
            return _navMeshAgent.remainingDistance;
        }

        Vector3 targetPosition = _currentTarget;
        Vector3 agentPosition = Agent.Value.transform.position;
        agentPosition.y = targetPosition.y; // Ignore y for distance check.
        return Vector3.Distance(agentPosition, targetPosition);
    }

    private void MoveToNextWaypoint()
    {
        var _currentKnot = Agent.Value.PatrolPaths[_currentPatrolPath].Spline[_currentPatrolPoint];
        var _currentKnotPosition = new Vector3(_currentKnot.Position.x, _currentKnot.Position.y, _currentKnot.Position.z);
        _currentTarget = Agent.Value.PatrolPaths[_currentPatrolPath].transform.position + _currentKnotPosition;// Waypoints.Value[_currentPatrolPoint].transform.position;
        if (_navMeshAgent != null)
            _navMeshAgent.SetDestination(_currentTarget);

        _currentPatrolPoint++;
        if (_currentPatrolPoint + 1 > Agent.Value.PatrolPaths[_currentPatrolPath].Spline.Count)
        {
            _currentPatrolPoint = 0;
            if (Agent.Value.PatrolPaths.Count > 1)
            {
                _currentPatrolPath++;
            }
            // if (Agent.Value.PatrolPaths[_currentPatrolPath].Spline.Closed)
            // {

            // }
        }
    }

    // private void UpdateAnimatorSpeed(float explicitSpeed = -1f)
    // {
    //     NavigationUtility.UpdateAnimatorSpeed(_animator, AnimatorSpeedParam, _navMeshAgent, _currentSpeed, explicitSpeed: explicitSpeed);
    // }
    
    private void OnDrawGizmos()
    {
        DrawGizmos(_navMeshAgent, true, true);
    }

    public static void DrawGizmos(NavMeshAgent agent, bool showPath, bool showAhead)
    {
        if (Application.isPlaying && agent != null)
        {
            if (showPath && agent.hasPath)
            {
                var corners = agent.path.corners;
                if (corners.Length < 2) { return; }
                int i = 0;
                for (; i < corners.Length - 1; i++)
                {
                    Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(agent.path.corners[i + 1], 0.03f);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
                }
                Debug.DrawLine(corners[0], corners[1], Color.blue);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(agent.path.corners[1], 0.03f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(agent.path.corners[0], agent.path.corners[1]);
            }

            if (showAhead)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(agent.transform.position, agent.transform.up * 0.5f);
            }
        }
    }
}


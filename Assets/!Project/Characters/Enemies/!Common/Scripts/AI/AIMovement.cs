using UnityEngine;
using Sirenix.OdinInspector;

public class AIMovement : MonoBehaviour
{
    #region Data
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [ReadOnly] public bool Sprint;
    #endregion

    #region References
    [HideInInspector] public AIBehaviour AI;
    #endregion

    private void Update()
    {
        Sprint = AI.NavigationMode == AI.Pursuit || AI.NavigationMode == AI.Search;
        AI.NavMeshAgent.speed = Sprint ? _sprintSpeed : _walkSpeed;
    }       
}

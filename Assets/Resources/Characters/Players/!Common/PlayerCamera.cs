using UnityEngine;
using Unity.Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PlayerCamera : MonoBehaviour
{
    #region Values
    [SerializeField] private float _playerWeight;
    [ReadOnly] private float _currentMousePositionWeight;
    [SerializeField] private float _mousePositionWeight;
    [SerializeField] private float _mousePositionFocusedWeight;
    [SerializeField] private float _focusTime;
    #endregion

    #region References
    public Player Player;
    private CinemachineTargetGroup _targetGroup;
    #endregion

    void Awake() 
    {
        _targetGroup = GetComponent<CinemachineTargetGroup>();
        _currentMousePositionWeight = _mousePositionWeight;
        _targetGroup.Targets[0].Weight = _playerWeight;
    }

    void Update()
    {
        _targetGroup.Targets[1].Weight = _currentMousePositionWeight;
    }

    public void Aim(bool isActive)
    {
        float targetWeight = isActive ? _mousePositionFocusedWeight : _mousePositionWeight;

        DOTween.To(() => _currentMousePositionWeight, 
                   x => _currentMousePositionWeight = x, 
                   targetWeight, 
                   _focusTime);
    }
}

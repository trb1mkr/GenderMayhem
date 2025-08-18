using UnityEngine;

public class TimeDebugger : MonoBehaviour
{
    [SerializeField] float _timeScale = 1;

    #if UNITY_EDITOR
    void OnValidate() => Time.timeScale = _timeScale;
    #endif

    void Awake() => Time.timeScale = _timeScale;
}

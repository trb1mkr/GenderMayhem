using UnityEngine;

public class TimeDebugger : MonoBehaviour
{
    [SerializeField] float _timeScale = 1;

    void OnValidate()
    {
        Time.timeScale = _timeScale;
    }
}

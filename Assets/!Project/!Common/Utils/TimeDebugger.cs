using UnityEngine;

public class TimeDebugger : MonoBehaviour
{
    [SerializeField] float _timeScale = 1;

    void Awake()
    {
        Time.timeScale = _timeScale;
    }
}

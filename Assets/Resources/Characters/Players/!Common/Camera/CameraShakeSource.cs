using UnityEngine;

public class CameraShakeSource : MonoBehaviour
{
    #region Values
    public float Duration;
    public float Amplitude;
    public float Frequency;
    #endregion

    CameraShakeController _cameraShakeController;

    void Awake()
    {
        _cameraShakeController = FindFirstObjectByType<CameraShakeController>();
    }

    public void Shake() => _cameraShakeController.Shake(Duration, Amplitude, Frequency);
}

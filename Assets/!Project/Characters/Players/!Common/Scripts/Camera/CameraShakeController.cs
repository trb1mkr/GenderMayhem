using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
    #region Values
    private float _shakeDuration = 0f;
    private float _shakeAmplitude = 0f;
    private float _shakeFrequency = 0f;
    #endregion

    #region References
    [SerializeField] CinemachineBasicMultiChannelPerlin _cameraShake;
    #endregion

    void Awake()
    {
        _cameraShake = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float duration, float amplitude, float frequency)
    {
        _shakeDuration = duration;
        _shakeAmplitude = amplitude;
        _shakeFrequency = frequency;

        _cameraShake.AmplitudeGain = amplitude;
        _cameraShake.FrequencyGain = frequency;
    }

    void Update()
    {
        if (_shakeDuration > 0)
        {
            _shakeDuration -= Time.deltaTime;
        }
        else
        {
            _cameraShake.AmplitudeGain = 0f;
            _cameraShake.FrequencyGain = 0f;
        }
    }
}

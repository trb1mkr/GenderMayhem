using UnityEngine;
using UnityEngine.Events;

public class AIHearing : MonoBehaviour, IAudioSourceListener
{
    [HideInInspector] public UnityEvent<GameObject, SoundEmitType> SoundDetected { get { return _soundDetected; } set {} }
    //[HideInInspector] public UnityEvent<GameObject, SoundEmitType> SoundDetected = new();
    private UnityEvent<GameObject, SoundEmitType> _soundDetected = new();

    #region References
    [HideInInspector] public AIBehaviour AI;
    #endregion
}

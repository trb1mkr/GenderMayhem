using UnityEngine;
using System;

public class AIHearing : MonoBehaviour, IAudioSourceListener
{
    #region Data
    [HideInInspector] public Action<GameObject, SoundEmitType> SoundDetected { get { return _soundDetected; } set { _soundDetected = value; } }
    private Action<GameObject, SoundEmitType> _soundDetected;
    #endregion

    #region References
    [HideInInspector] public AIDetection Detection;
    #endregion
}

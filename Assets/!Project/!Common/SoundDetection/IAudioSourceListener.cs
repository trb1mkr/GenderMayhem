using UnityEngine;
using UnityEngine.Events;

public interface IAudioSourceListener
{
    public UnityEvent<GameObject, SoundEmitType> SoundDetected { get; set; }
}

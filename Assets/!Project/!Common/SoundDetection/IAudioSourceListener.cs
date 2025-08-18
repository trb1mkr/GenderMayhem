using UnityEngine;
using System;

public interface IAudioSourceListener
{
    public Action<GameObject, SoundEmitType> SoundDetected { get; set; }
}

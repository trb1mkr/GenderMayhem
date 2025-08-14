using UnityEngine;

public interface IAudioSourceListener
{
    public void OnSoundDetected(GameObject source, SoundEmitType soundEmitType);
}

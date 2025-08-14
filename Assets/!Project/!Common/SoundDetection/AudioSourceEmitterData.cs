using UnityEngine;

[CreateAssetMenu(fileName = "SoundEmitData", menuName = "ScriptableObjects/SoundEmitData", order = 1)]
public class SoundEmitData : ScriptableObject
{
    public float SoundRadius;
    public LayerMask ListenerMask;
    public SoundEmitType SoundType;
}

public enum SoundEmitType
{
    Shot,
    MeleeHit
}

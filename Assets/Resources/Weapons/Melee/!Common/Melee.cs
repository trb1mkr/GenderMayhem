using UnityEngine;

public abstract class Melee : Weapon
{
    public AudioClip HitSound;
    public bool Lethal;
    public float AttackTime;
    public float CooldownTime;
    [HideInInspector] public bool Cooldown = false;

    private new void Start()
    {
        base.Start();
    }
}

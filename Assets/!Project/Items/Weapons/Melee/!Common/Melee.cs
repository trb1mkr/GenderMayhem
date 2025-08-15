using UnityEngine;

public abstract class Melee : Weapon
{
    public AudioClip HitSound;
    [SerializeField] protected SoundEmitData HitSoundEmitData;
    public bool IsLethal;
    public float AttackTime;
    public float CooldownTime; //с учётом длительности анимации

    public override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        AudioSource.PlayOneShot(AttackSound);
        base.Attack();
    }
}

using UnityEngine;

public abstract class Melee : Weapon
{
    public AudioClip HitSound;
    public bool Lethal;
    public float AttackTime;
    public float CooldownTime; //с учётом длительности анимации

    public override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        //if (IsCooldown) return;
        AudioSource.PlayOneShot(AttackSound);
        //StartCoroutine(Cooldown());
        base.Attack();
    }
}

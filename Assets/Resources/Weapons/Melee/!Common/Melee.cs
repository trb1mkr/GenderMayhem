using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public abstract class Melee : Weapon
{
    public AudioClip HitSound;
    public bool Lethal;
    public float AttackTime;
    public float CooldownTime; //с учётом длительности анимации
    [ReadOnly] public bool IsCooldown;

    private new void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        if (IsCooldown) return;
        AudioSource.PlayOneShot(AttackSound);
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        IsCooldown = true;
        yield return new WaitForSeconds(CooldownTime);
        IsCooldown = false;
    }
}

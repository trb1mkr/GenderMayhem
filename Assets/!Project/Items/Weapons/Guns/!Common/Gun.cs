using System.Collections;
using UnityEngine;
using GenderMayhem.Actions;
using System.Collections.Generic;
using UnityEngine.Events;

public abstract class Gun : Weapon
{
    public AudioClip ShotSound;
    public GunUtilities GunUtilities = new GunUtilities();
    public int Ammo, Spread;

    public override void Awake()
    {
        base.Awake();
        var altUseActions = new List<UnityAction> { new(AltAttack) };
        ActionEventsGroup.ActionEvents.Add(new ActionEvent(typeof(PlayerInputAction), PlayerInputAction.AltUse, altUseActions));
    }

    public override void Start()
    {
        base.Start();
        GunUtilities.ShellPoint = gameObject.transform.GetChild(0).gameObject;
        GunUtilities.BulletPoint = gameObject.transform.GetChild(1).gameObject;
        GunUtilities.MuzzleFlash = gameObject.transform.GetChild(2).gameObject;
    }

    public bool Shoot()
    {
        if (Ammo > 0)
        {
            Ammo--;
            Fire();
            StartCoroutine(MuzzleFlash());
            AudioSource.PlayOneShot(ShotSound);
            return true;
        }
        return false;
    }

    public override void Attack()
    {
        if (Shoot())
            base.Attack();
    }

    public void AltAttack()
    {
        AudioSource.PlayOneShot(AttackSound);
        //StartCoroutine(Cooldown());
    }

    // IEnumerator Cooldown()
    // {
    //     IsCooldown = true;
    //     yield return new WaitForSeconds(CooldownTime);
    //     IsCooldown = false;
    // }

    virtual public void Fire() { }

    //virtual public void NoAmmo() { }

    IEnumerator MuzzleFlash()
    {
        GunUtilities.MuzzleFlash.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        GunUtilities.MuzzleFlash.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SpawnBullet() =>
        Instantiate(GunUtilities.Bullet, GunUtilities.BulletPoint.transform.position, Quaternion.Euler(GunUtilities.BulletPoint.transform.eulerAngles + new Vector3(0, 0, Random.Range(-Spread, Spread))), transform);

    public void SpawnShell() =>
        Instantiate(GunUtilities.Shell, GunUtilities.ShellPoint.transform.position, GunUtilities.ShellPoint.transform.rotation);

    public void SpawnMagazine() =>
        Instantiate(GunUtilities.Magazine, gameObject.transform.position, gameObject.transform.rotation);
}

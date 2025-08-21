using System.Collections;
using UnityEngine;
using GenderMayhem.Actions;
using System.Collections.Generic;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;
using R3;
using Random = UnityEngine.Random;

public abstract class Gun : Weapon
{
    public AudioClip ShotSound;
    public AudioClip ReloadSound;
    [SerializeField] protected SoundEmitData ShotSoundEmitData;

    public int MaxAmmo;
    public SerializableReactiveProperty<int> Ammo;
    public Action AmmoIsOut;
    public float ReloadTime;
    [SerializeField] protected float CycleTime;
    [ReadOnly] public bool IsCycling = false;

    public GunUtilities GunUtilities = new();
    public int Spread;

    public override void Awake()
    {
        base.Awake();
        var altUseActions = new List<UnityAction> { new(AltAttack) };
        ActionEventsGroup.ActionEvents.Add(new ActionEvent(typeof(ItemAction), ItemAction.AltUse, altUseActions));

        var reloadActions = new List<UnityAction> { new(() => StartCoroutine(Reload())) };
        ActionEventsGroup.ActionEvents.Add(new ActionEvent(typeof(WeaponAction), WeaponAction.Reload, reloadActions));

        Ammo.Value = MaxAmmo;

        Ammo
            .Where(ammo => ammo < 1)
            .Subscribe(_ => AmmoIsOut?.Invoke());
    }

    public override void Start()
    {
        base.Start();
        GunUtilities.ShellPoint = gameObject.transform.GetChild(0).gameObject;
        GunUtilities.BulletPoint = gameObject.transform.GetChild(1).gameObject;
        GunUtilities.MuzzleFlash = gameObject.transform.GetChild(2).gameObject;
    }

    public override void Attack()
    {
        if (Shoot())
            base.Attack();
    }

    public bool Shoot()
    {
        if (Ammo.Value < 1 || IsCycling) return false;

        Ammo.Value--;
        Fire();
        StartCoroutine(MuzzleFlash());
        AudioSource.PlayOneShot(ShotSound);
        AudioSourceEmitter.NotifyListeners(ShotSoundEmitData);
        StartCoroutine(Cycle());
        return true;
    }

    virtual public void Fire() { }

    virtual public IEnumerator Cycle()
    {
        IsCycling = true;
        yield return new WaitForSeconds(CycleTime);
        IsCycling = false;
    }

    public void AltAttack()
    {
        AudioSource.PlayOneShot(AttackSound);
    }

    virtual public IEnumerator Reload()
    {
        AudioSource.PlayOneShot(ReloadSound);
        yield return new WaitForSeconds(ReloadTime);
        SpawnMagazine();
        Ammo.Value = MaxAmmo;
    }

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

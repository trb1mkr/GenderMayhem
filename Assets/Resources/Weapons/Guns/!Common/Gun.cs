using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Weapon
{
    public AudioClip ShotSound;
    public GunUtilities GunUtilities = new GunUtilities();
    public int Ammo, Spread;

    new void Start()
    {
        base.Start();
        GunUtilities.ShellPoint = gameObject.transform.GetChild(0).gameObject;
        GunUtilities.BulletPoint = gameObject.transform.GetChild(1).gameObject;
        GunUtilities.MuzzleFlash = gameObject.transform.GetChild(2).gameObject;
    }

    public void Shoot()
    {
        if (Ammo > 0)
        {
            Ammo--;
            Fire();
            StartCoroutine(MuzzleFlash());
            AudioSource.PlayOneShot(ShotSound);
        }
        if (Ammo == 0)
        {
            NoAmmo();
            GunUtilities.Magazine = null;
        }
        return;
    }

    virtual public void Fire() { return; }
    virtual public void NoAmmo() { return; }

    IEnumerator MuzzleFlash()
    {
        GunUtilities.MuzzleFlash.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        GunUtilities.MuzzleFlash.GetComponent<SpriteRenderer>().enabled = false;
        yield break;
    }

    public void SpawnBullet()
    {
        Instantiate(GunUtilities.Bullet, GunUtilities.BulletPoint.transform.position, Quaternion.Euler(GunUtilities.BulletPoint.transform.eulerAngles + new Vector3(0, 0, Random.Range(-Spread, Spread))));
    }

    public void SpawnShell()
    {
        Instantiate(GunUtilities.Shell, GunUtilities.ShellPoint.transform.position, GunUtilities.ShellPoint.transform.rotation);
    }

    public void SpawnMagazine()
    {
        Instantiate(GunUtilities.Magazine, gameObject.transform.position, gameObject.transform.rotation);
    }
}

[System.Serializable]
public class GunUtilities
{
    [HideInInspector] public GameObject BulletPoint;
    [HideInInspector] public GameObject ShellPoint;
    [HideInInspector] public GameObject MuzzleFlash;
    public GameObject Bullet;
    public GameObject Shell;
    public GameObject Magazine;
}

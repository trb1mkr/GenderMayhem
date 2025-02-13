using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Weapon
{
    public AudioClip shotSound;
    public GunUtilities gunUtilities = new GunUtilities();
    public int ammo, spread;

    private new void Start()
    {
        base.Start();
        gunUtilities.shellPoint = gameObject.transform.GetChild(0).gameObject;
        gunUtilities.bulletPoint = gameObject.transform.GetChild(1).gameObject;
        gunUtilities.muzzleFlash = gameObject.transform.GetChild(2).gameObject;
    }

    public void Shoot() //ÌÎÆÍÎ ÆÅ ÎÂÅÐÐÀÉÄÈÒÜ È ÌÅÒÎÄÛ Ñ ÊÎÄÎÌ
    {
        if (ammo > 0)
        {
            ammo--;
            Fire();
            StartCoroutine(muzzleFlash());
            audioSource.PlayOneShot(shotSound);
        }
        if (ammo == 0)
        {
            NoAmmo();
            gunUtilities.magazine = null;
        }
        return;
    }

    virtual public void Fire() { return; }
    virtual public void NoAmmo() { return; }

    IEnumerator muzzleFlash()
    {
        gunUtilities.muzzleFlash.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        gunUtilities.muzzleFlash.GetComponent<SpriteRenderer>().enabled = false;
        yield break;
    }

    public void SpawnBullet()
    {
        Instantiate(gunUtilities.bullet, gunUtilities.bulletPoint.transform.position, Quaternion.Euler(gunUtilities.bulletPoint.transform.eulerAngles + new Vector3(0, 0, Random.Range(-spread, spread))));
    }

    public void SpawnShell()
    {
        Instantiate(gunUtilities.shell, gunUtilities.shellPoint.transform.position, gunUtilities.shellPoint.transform.rotation);
    }

    public void SpawnMagazine()
    {
        Instantiate(gunUtilities.magazine, gameObject.transform.position, gameObject.transform.rotation);
    }
}

[System.Serializable]
public class GunUtilities
{
    [HideInInspector] public GameObject bulletPoint;
    [HideInInspector] public GameObject shellPoint;
    [HideInInspector] public GameObject muzzleFlash;
    public GameObject bullet;
    public GameObject shell;
    public GameObject magazine;
}

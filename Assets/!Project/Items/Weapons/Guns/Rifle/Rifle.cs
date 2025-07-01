using System.Collections;
using UnityEngine;

public class Rifle : Gun
{
    [SerializeField] private float _rateOfFire;

    public override void Fire()
    {
        SpawnBullet();
        SpawnShell();
        //Instantiate(GunUtilities.Bullet, GunUtilities.BulletPoint.transform.position, GunUtilities.BulletPoint.transform.rotation);
        //Instantiate(GunUtilities.Shell, GunUtilities.ShellPoint.transform.position, GunUtilities.ShellPoint.transform.rotation);
    }

    public override IEnumerator Use()
    {
        while (Ammo > 0)
        {
            yield return base.Use();
            yield return new WaitForSeconds(_rateOfFire);
        }
    }

    // public override void NoAmmo()
    // {
    //     Instantiate(GunUtilities.Magazine, gameObject.transform.position, gameObject.transform.rotation);
    // }
}

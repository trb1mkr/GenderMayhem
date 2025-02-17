using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    public override void Fire()
    {
        Instantiate(GunUtilities.Bullet, GunUtilities.BulletPoint.transform.position, GunUtilities.BulletPoint.transform.rotation);
        Instantiate(GunUtilities.Shell, GunUtilities.ShellPoint.transform.position, GunUtilities.ShellPoint.transform.rotation);
    }
    public override void NoAmmo()
    {
        Instantiate(GunUtilities.Magazine, gameObject.transform.position, gameObject.transform.rotation);
    }
}

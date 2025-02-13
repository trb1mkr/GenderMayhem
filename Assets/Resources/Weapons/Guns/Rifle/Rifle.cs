using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    public override void Fire()
    {
        Instantiate(gunUtilities.bullet, gunUtilities.bulletPoint.transform.position, gunUtilities.bulletPoint.transform.rotation);
        Instantiate(gunUtilities.shell, gunUtilities.shellPoint.transform.position, gunUtilities.shellPoint.transform.rotation);
    }
    public override void NoAmmo()
    {
        Instantiate(gunUtilities.magazine, gameObject.transform.position, gameObject.transform.rotation);
    }
}

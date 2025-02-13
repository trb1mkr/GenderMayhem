using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Fire()
    {
        SpawnBullet();
        SpawnShell();
    }
    public override void NoAmmo()
    {
        SpawnMagazine();
    }
}

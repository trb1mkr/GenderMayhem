using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public override void Fire()
    {
        for (int i = 0; i < 10; i++)
        {
            SpawnBullet();
        }
    }
    public override void NoAmmo()
    {
        SpawnShell();
        SpawnShell();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Melee : Weapon
{
    public AudioClip hitSound;
    public bool lethal;
    public float attackTime;
    public float cooldownTime;
    [HideInInspector] public bool cooldown = false;

    private new void Start()
    {
        base.Start();
    }
}

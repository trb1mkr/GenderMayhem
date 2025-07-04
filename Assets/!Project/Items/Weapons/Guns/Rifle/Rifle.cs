using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using GenderMayhem.Actions;

public class Rifle : Gun
{
    [SerializeField] private float _rateOfFire;
    private Coroutine _autoFire;

    public override void Awake()
    {
        base.Awake();

        var useCanceledActions = new List<UnityAction> { new(StopAttack) };
        ActionEventsGroup.ActionEvents.Add(new ActionEvent(typeof(PlayerInputAction), PlayerInputAction.UseCanceled, useCanceledActions));
    }

    public override void Fire()
    {
        SpawnBullet();
        SpawnShell();
    }

    public override void Attack() =>
        _autoFire = StartCoroutine(AutoAttack());

    public void StopAttack()
    {
        if (_autoFire != null) StopCoroutine(_autoFire);
    }

    public IEnumerator AutoAttack()
    {
        while (Ammo > 0)
        {
            base.Attack();
            yield return new WaitForSeconds(_rateOfFire);
        }
    }

    // public override void NoAmmo()
    // {
    //     Instantiate(GunUtilities.Magazine, gameObject.transform.position, gameObject.transform.rotation);
    // }
}

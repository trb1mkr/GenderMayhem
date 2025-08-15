using UnityEngine;
using GenderMayhem.Actions;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
abstract public class Weapon : Item
{
    #region Values
    public AudioClip AttackSound;

    [HideInInspector] public UnityEvent Attacked;
    #endregion

    public virtual void Awake()
    {
        var useActions = new List<UnityAction> { new(Attack) };
        ActionEventsGroup.ActionEvents.Add(new ActionEvent(typeof(PlayerInputAction), PlayerInputAction.Use, useActions));
    }

    public virtual void Attack()
    {
        Attacked.Invoke();
        GetComponent<CameraShakeSource>().Shake();
    }
}
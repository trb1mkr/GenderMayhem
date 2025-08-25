using UnityEngine;
using GenderMayhem.Actions;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

[Serializable]
abstract public class Weapon : Item
{
    #region Values
    public float AttackDistance;
    public AudioClip AttackSound;

    [HideInInspector] public UnityEvent Attacked;
    #endregion

    public virtual void Awake()
    {
        var useActions = new List<UnityAction> { new(Attack) };
        ActionEventsGroup.ActionEvents.Add(new ActionEvent(typeof(ItemAction), ItemAction.Use, useActions));
    }

    public virtual void Attack()
    {
        Debug.Log("Attacked");
        if (GetComponentInParent<Player>() != null) GetComponent<CameraShakeSource>().Shake();
        Attacked.Invoke();
    }
}
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections;
using Unity.VisualScripting;

public class CharacterStateController : MonoBehaviour
{
    #region Values
    public CharacterStateId StateId;
    [field: ReadOnly] public WeaponId WeaponId { get; private set; }
    public PolygonCollider2D TorsoCollider;
    public PolygonCollider2D AttackCollider;
    public PolygonCollider2D AvoidCollider;
    public Transform WeaponPoint;
    #endregion

    #region References
    [HideInInspector] public Character Character;
    #endregion

    public void Start()
    {
        SetWeaponId();
        StateId = CharacterStateId.Idle;
        Character.ItemManager.ItemChanged += SetWeaponId;
        Character.WeaponController.CooldownEnded += OnCooldownEnded;
        Character.ItemManager.ItemThrowed += OnItemThrowed;
        Character.AnimatorController.AttackAnimationStarted += time => StartCoroutine(EndAttack(time));
    }

    private void Update()
    {
        if (Character.AvoidList.Colliders.Count > 0)
            StateId = CharacterStateId.Avoid;
        else if (StateId == CharacterStateId.Avoid)
            StateId = CharacterStateId.Idle;
    }

    void SetWeaponId()
    {
        if (Enum.TryParse(((Weapon)Character.ItemManager.Item).GetType().Name, out WeaponId weaponId))
            WeaponId = weaponId;
        else
            throw new ArgumentException($"Weapon type '{((Weapon)Character.ItemManager.Item).GetType().Name}' not found in WeaponId enum.");
    }

    public void OnAltUsed()
    {
        if (Character.ItemManager.Item is Gun)
            StateId = CharacterStateId.Attack;
    }

    public void OnUsed()
    {
        if (Character.ItemManager.Item is Melee)
            StateId = CharacterStateId.Attack;
    }

    public void OnItemThrowed() => StateId = CharacterStateId.Idle;

    private void OnCooldownEnded() => StateId = CharacterStateId.Idle;

    private IEnumerator EndAttack(float time)
    {
        yield return new WaitForSeconds(time);
        StateId = CharacterStateId.Idle;
    }
}

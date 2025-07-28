using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class CharacterStateController : MonoBehaviour
{
    #region Values
    public CharacterStateId StateId;
    [field: ReadOnly] public WeaponId WeaponId { get; private set; }
    public PolygonCollider2D BodyCollider;
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
        Character.WeaponController.CooldownEnded += HandleCooldownEnded;
        Character.ItemManager.ItemThrowed += HandleItemThrowed;
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

    public void HandleAltUsed()
    {
        //if (Character.ItemManager.Item is Gun)
        //StateId = CharacterStateId.Attack;
    }

    public void HandleUsed()
    {
        if (Character.ItemManager.Item is Melee)
            StateId = CharacterStateId.Attack;
    }

    public void HandleItemThrowed() => StateId = CharacterStateId.Idle;

    private void HandleCooldownEnded() => StateId = CharacterStateId.Idle;
}

public enum WeaponId
{
    Fists = 0,
    Knife = 1,
    Bat = 2,
    Katana = 3,
    Pistol = 4,
    Shotgun = 5,
    Rifle = 6
}

public enum CharacterStateId
{
    Idle = 0,
    Attack = 1,
    Avoid = 2
}
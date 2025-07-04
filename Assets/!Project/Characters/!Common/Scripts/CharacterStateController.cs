using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class CharacterStateController : MonoBehaviour
{
    public CharacterStateId StateId;
    [field: ReadOnly] public WeaponId WeaponId { get; private set; }

    [HideInInspector] public Character Character;

    public void Start()
    {
        SetWeaponId();
        Character.ItemManager.OnItemChange.AddListener(SetWeaponId);
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
        if (Character.ItemManager.Item is Weapon)
            StateId = CharacterStateId.Attack;
    }

    public void HandleUsed()
    {
        if (Character.ItemManager.Item is Melee)
            StateId = CharacterStateId.Attack;
    }
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
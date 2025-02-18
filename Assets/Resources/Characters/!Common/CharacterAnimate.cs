using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterAnimate : MonoBehaviour
{
    #region Values
    [ReadOnly] public WeaponId Weapon;
    [ReadOnly] public CharacterStateId State;
    [ReadOnly] public bool Dead;
    [ReadOnly] public bool Unconscious;
    #endregion

    #region References
    [HideInInspector] public Character Character;
    [SerializeField] Animator _animator;
    #endregion

    void Update()
    {
        Animate();
    }

    public void Animate()
    {
        //string weaponName = Character.Items.weapon.GetType().Name;

        _animator.SetInteger("Weapon", (int)Weapon);
        _animator.SetInteger("State", (int)State);
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

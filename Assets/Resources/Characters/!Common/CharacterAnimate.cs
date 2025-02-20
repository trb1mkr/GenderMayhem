using UnityEngine;

public class CharacterAnimate : MonoBehaviour
{
    #region References
    [HideInInspector] public Character Character;
    [SerializeField] Animator _animator;
    #endregion

    void Update()
    {
        Animate();
    }

    //[OnInspectorGUI]
    public void Animate()
    {
        _animator.SetInteger("Weapon", (int)Character.Hands.WeaponId);
        _animator.SetInteger("State", (int)Character.StateId);
        _animator.SetBool("Dead", Character.Dead);
        _animator.SetBool("Unconscious", Character.Unconscious);
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

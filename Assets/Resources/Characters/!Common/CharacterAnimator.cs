using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    #region References
    [HideInInspector] public Character Character;
    public Animator Animator;
    #endregion

    void Update()
    {
        Animate();
    }

    //[OnInspectorGUI]
    public void Animate()
    {
        Animator.SetInteger("Weapon", (int)Character.ItemManager.WeaponId);
        Animator.SetInteger("State", (int)Character.StateId);
        Animator.SetBool("Dead", Character.Dead);
        Animator.SetBool("Unconscious", Character.Unconscious);
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

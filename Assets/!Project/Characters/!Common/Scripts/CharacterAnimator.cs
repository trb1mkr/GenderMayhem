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
        Animator.SetInteger("Weapon", (int)Character.StateController.WeaponId);
        Animator.SetInteger("State", (int)Character.StateController.StateId);
        Animator.SetBool("Dead", Character.Dead);
        Animator.SetBool("Unconscious", Character.Unconscious);
    }
}
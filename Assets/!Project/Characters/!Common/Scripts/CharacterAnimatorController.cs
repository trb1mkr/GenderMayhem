using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    #region References
    [HideInInspector] public Character Character;
    #endregion

    void Update()
    {
        Animate();
    }

    //[OnInspectorGUI]
    public void Animate()
    {
        Character.Animator.SetInteger("Weapon", (int)Character.StateController.WeaponId);
        Character.Animator.SetInteger("State", (int)Character.StateController.StateId);
        Character.Animator.SetBool("Dead", Character.Health.Dead);
        Character.Animator.SetBool("Unconscious", Character.Health.Unconscious);
    }
}
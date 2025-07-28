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
        Character.Animator.SetBool("Lying", Character.Health.IsDead || Character.Health.IsUnconscious);
        Character.Animator.SetBool("OnBack", Character.Health.IsAttackedFromFront);
        Character.Animator.SetBool("Dead", Character.Health.IsDead);
        Character.Animator.SetInteger("Weapon", (int)Character.StateController.WeaponId);
        Character.Animator.SetInteger("State", (int)Character.StateController.StateId);
    }
}
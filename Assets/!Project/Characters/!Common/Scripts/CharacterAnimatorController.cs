using UnityEngine;
using System;

public class CharacterAnimatorController : MonoBehaviour
{
    public Action<float> AttackAnimationStarted;

    #region References
    [HideInInspector] public Character Character;
    #endregion

    void Update()
    {
        PlayState(MakeStateName());
    }

    public string MakeStateName()
    {
        string name = "";
        if (Character.Health.IsDead || Character.Health.IsUnconscious)
        {
            name = "Lying";
            if (Character.Health.IsAttackedFromFront) name += "Back";
            else name += "Stomach";
            if (Character.Health.IsDead) name += "Dead";
            else name += "Unconscious";
        }
        else
        {
            name += Character.StateController.WeaponId.ToString();
            name += Character.StateController.StateId.ToString();
        }
        return name;
    }

    void PlayState(string newStateName)
    {
        if (Character.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == newStateName) return;
        Character.Animator.Play(newStateName);
        if (newStateName.Contains("Attack")) AttackAnimationStarted?.Invoke(Character.Animator.GetCurrentAnimatorClipInfo(0).Length);
    }
}
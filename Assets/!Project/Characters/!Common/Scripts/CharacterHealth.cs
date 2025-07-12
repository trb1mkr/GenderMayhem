using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System;

public class CharacterHealth : MonoBehaviour
{
    #region Values
    public float KnockdownForce;
    public float KnockdownTime;
    [ReadOnly] public bool Dead;
    [ReadOnly] public bool Unconscious;
    public event Action Died;
    public event Action Knocked;
    public event Action StoodUp;
    #endregion

    #region References
    [HideInInspector] public Character Character;
    #endregion

    [Button]
    public void Die()
    {
        Unconscious = false;
        Dead = true;
        Character.ItemManager.Throw();
        Character.Rigidbody.simulated = false;
        StopCoroutine(StandUp());
        ChangeComponentState(false);
        Died?.Invoke();
    }

    [Button]
    public void Knockdown()
    {
        Unconscious = true;
        Character.ItemManager.Throw();
        Character.Rigidbody.simulated = false;
        StartCoroutine(StandUp());
        ChangeComponentState(false);
        Knocked?.Invoke();
    }

    private void ChangeComponentState(bool state)
    {

    }

    public IEnumerator StandUp()
    {
        yield return new WaitForSeconds(KnockdownTime);
        Unconscious = false;
        Character.Rigidbody.simulated = true;
        ChangeComponentState(true);
        StoodUp?.Invoke();
    }
}

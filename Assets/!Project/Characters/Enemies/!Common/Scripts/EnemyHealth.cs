using UnityEngine;

public class EnemyHealth : CharacterHealth
{
    protected override void ChangeComponentsState(bool state)
    {
        ((Enemy)Character).AI.enabled = state;
        base.ChangeComponentsState(state);
    }
}

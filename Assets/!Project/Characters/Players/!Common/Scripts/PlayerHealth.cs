using UnityEngine.InputSystem;

public class PlayerHealth : CharacterHealth
{
    protected override void ChangeComponentsState(bool state)
    {
        base.ChangeComponentsState(state);
        ((Player)Character).GetComponent<PlayerInput>().enabled = state;
    }
}

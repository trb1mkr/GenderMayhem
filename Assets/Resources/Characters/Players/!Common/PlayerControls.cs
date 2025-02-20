using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    #region Values
    public Vector2 MousePosition { get; private set; }
    #endregion

    #region References
    [HideInInspector] public Player Player;
    #endregion

    public void OnMove(InputAction.CallbackContext context) =>
        Player.Movement.MovementDirection = context.ReadValue<Vector2>();

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Player.Hands.Weapon is Melee)
                Player.StateId = CharacterStateId.Attack;
            else if (Player.Hands.Weapon is Gun)
            {
                Player.Hands.Weapon.Attack();
            }
        }
            // if (Player.Weapon != null) 
            // {
            //     //Player.Weapon.Attack();

            // }
    }

    public void OnStockAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            Player.StateId = CharacterStateId.Attack;
    }

    // public void OnThrow(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //         Player.Hands.Throw();
    // }

    // public void OnPickUp(InputAction.CallbackContext context)
    // {
    //     if (context.started)
    //         Player.Hands.PickUp();
    // }

    public void OnPickUpThrow(InputAction.CallbackContext context)
    {
        if (context.started)
            Player.Hands.PickUp();
    }

    public void OnFinishOff(InputAction.CallbackContext context) { }

    public void OnAim(InputAction.CallbackContext context) =>
        Player.Camera.Aim(context.performed);

    public void OnAutomaticFire(InputAction.CallbackContext context) {}

    public void OnMousePosition(InputAction.CallbackContext context) => 
        MousePosition = context.ReadValue<Vector2>();

    public void OnPause(InputAction.CallbackContext context) {}
}

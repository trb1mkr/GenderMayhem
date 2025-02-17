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
            if (Player.Weapon != null) Player.Weapon.Attack();
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.performed)
            Player.Items.PickUp();
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.performed)
            Player.Items.Throw();
    }

    public void OnFinishOff(InputAction.CallbackContext context) { }

    public void OnAim(InputAction.CallbackContext context) =>
        Player.Camera.Aim(context.performed);

    public void OnAutomaticFire(InputAction.CallbackContext context) {}

    public void OnMousePosition(InputAction.CallbackContext context) => 
        MousePosition = context.ReadValue<Vector2>();

    public void OnPause(InputAction.CallbackContext context) {}
}

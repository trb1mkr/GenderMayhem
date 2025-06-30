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

    public void OnUse(InputAction.CallbackContext context)
    {
        // if (context.performed)
        // {
        //     Player.ItemManager.Use();
        //     // if (Player.Hands.Weapon is Melee)
        //     //     if (Player.StateId != CharacterStateId.Attack)
        //     //         Player.StateId = CharacterStateId.Attack;

        //     // if (Player.Hands.Weapon is Gun)
        //     //     Player.Hands.Weapon.Attack();
        // }
        // if (context.canceled) Debug.Log("cancelled");
        //Coroutine UseRoutine = null;

        if (context.performed) Player.ItemManager.Use();
        if (context.canceled && Player.ItemManager.UseRoutine != null) StopCoroutine(Player.ItemManager.UseRoutine);
    }

    public void OnAltUse(InputAction.CallbackContext context) //переименовать в AltUse
    {
        if (context.performed)
            Player.ItemManager.AltUse();
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
        if (context.performed)
            Player.ItemManager.PickUp();
    }

    public void OnFinishOff(InputAction.CallbackContext context) { }

    public void OnAim(InputAction.CallbackContext context) =>
        Player.Camera.Aim(context.performed);

    public void OnMousePosition(InputAction.CallbackContext context) => 
        MousePosition = context.ReadValue<Vector2>();

    public void OnPause(InputAction.CallbackContext context) {}
}

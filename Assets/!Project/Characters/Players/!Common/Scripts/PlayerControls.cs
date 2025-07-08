using GenderMayhem.Actions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    #region Values
    [HideInInspector] public UnityEvent Used, UseCanceled, AltUsed;
    public Vector2 MousePosition { get; private set; }
    #endregion

    #region References
    [HideInInspector] public Player Player;
    #endregion

    private void Start() //потом переснести отсюда
    {
        AltUsed.AddListener(Player.StateController.HandleAltUsed);
        Used.AddListener(Player.StateController.HandleUsed);
    }

    public void OnMove(InputAction.CallbackContext context) =>
        Player.Movement.MovementDirection = context.ReadValue<Vector2>();

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Player.WeaponController.IsCooldown) return;
            if (Player.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(PlayerInputAction.Use))
                Used.Invoke();
        }
        if (context.canceled)
            if (Player.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(PlayerInputAction.UseCanceled))
                UseCanceled.Invoke();
    }

    public void OnAltUse(InputAction.CallbackContext context)
    {
        if (context.performed && Player.StateController.StateId == CharacterStateId.Idle)
        {
            if (Player.ItemManager.Item.ActionEventsGroup.InvokeSuitableActions(PlayerInputAction.AltUse))
                AltUsed.Invoke();
        }
    }

    public void OnPickUpThrow(InputAction.CallbackContext context)
    {
        if (context.performed)
            Player.ItemManager.PickUp();
    }

    public void OnFinishOff(InputAction.CallbackContext context) { }

    public void OnAim(InputAction.CallbackContext context) =>
        Player.CamAimController.Aim(context.performed);

    public void OnMousePosition(InputAction.CallbackContext context) => 
        MousePosition = context.ReadValue<Vector2>();

    public void OnPause(InputAction.CallbackContext context) {}
}

using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerMovement : MonoBehaviour
{
    #region Values
    public int MovementSpeed;
    [ReadOnly] public Vector2 MovementDirection;
    #endregion

    #region References
    [HideInInspector] public Player Player;
    #endregion

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    public void Move()
    {
        //Player.Rigidbody.linearVelocity = new Vector2(MovementDirection.x * MovementSpeed, MovementDirection.y * MovementSpeed);
        Player.Rigidbody.AddForce(new Vector2(MovementDirection.x * MovementSpeed, MovementDirection.y * MovementSpeed) * 1000f, ForceMode2D.Force);
        if (MovementDirection.magnitude == 0) Player.Rigidbody.linearVelocity = Vector2.zero;
    }

    void Rotate()
    {
        var targetPosition = Camera.main.ScreenToWorldPoint(new Vector2(Player.Controls.MousePosition.x, Player.Controls.MousePosition.y - Player.CamAimController.transform.position.z));
        var targetDirection = (targetPosition - transform.position).normalized;
        var targetRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Player.Rigidbody.rotation = targetRotation; //забавное поведение с .MoveRotation()
    }
}

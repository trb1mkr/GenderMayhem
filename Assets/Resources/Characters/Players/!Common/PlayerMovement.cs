using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Values
    public int MovementSpeed;
    public int RotationSpeed;
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
        Player.Rigidbody.linearVelocity = new Vector2(MovementDirection.x * MovementSpeed, MovementDirection.y * MovementSpeed);
        //rb.AddForce(new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed) * 1000f, ForceMode2D.Force);
    }

    void Rotate()
    {
        var targetPosition = Camera.main.ScreenToWorldPoint(new Vector2(Player.Controls.MousePosition.x, Player.Controls.MousePosition.y - Player.Camera.transform.position.z));
        Player.Rigidbody.rotation = Mathf.Atan2((targetPosition.y - transform.position.y), (targetPosition.x - transform.position.x)) * Mathf.Rad2Deg;
    }
}

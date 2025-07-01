using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Force;

    void Awake()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody2D>();
        rigidBody.IgnoreCollisions(GetComponentInParent<Character>().Rigidbody, true);
        transform.parent = null;

        rigidBody.AddForce(new Vector2(gameObject.transform.right.x, gameObject.transform.right.y) * Force, ForceMode2D.Force);
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Shell>() || collider.GetComponent<Bullet>() || collider.GetComponent<Weapon>()) return;

        Destroy(gameObject);
    }
}

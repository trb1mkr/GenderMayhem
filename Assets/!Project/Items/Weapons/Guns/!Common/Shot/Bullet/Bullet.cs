using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _force;

    void Awake()
    {
        var rb = gameObject.GetComponent<Rigidbody2D>();
        rb.IgnoreCollisions(GetComponentInParent<Character>().Rigidbody, true);
        transform.parent = null;

        rb.AddForce(new Vector2(gameObject.transform.right.x, gameObject.transform.right.y) * _force, ForceMode2D.Force);
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Shell>() || collider.GetComponent<Bullet>() || collider.GetComponent<Weapon>()) return; //летит дальше

        var character = collider.GetComponentInParent<Character>();
        if (character != null && collider == character.StateController.TorsoCollider)
            character.Health.Die(transform.position);
        else return;

        Destroy(gameObject);
    }
}

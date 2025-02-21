using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Force;

    void Start()
    {
        //gameObject.transform.parent = GameManager.Bullets.transform;
        var BulletRigidBody = gameObject.GetComponent<Rigidbody2D>();
        BulletRigidBody.AddForce(new Vector2(gameObject.transform.right.x, gameObject.transform.right.y) * Force, ForceMode2D.Force);
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //try { if (collision.gameObject.GetComponent<Bullet>() != null) return; } catch { }
        //try { if (collision.tag == "Player" || collision.transform.parent.tag == "Player") return; } catch { }
        Destroy(gameObject);
    }
}

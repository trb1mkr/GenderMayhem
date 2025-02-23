using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int Force;

    void Awake()
    {
        //gameObject.transform.parent = GameManager.Bullets.transform;
        var rigidBody = gameObject.GetComponent<Rigidbody2D>();
        //rigidBody.IgnoreCollisions(GetComponentInParent<Weapon>().Rigidbody, true);
        rigidBody.IgnoreCollisions(GetComponentInParent<Character>().Rigidbody, true);
        transform.parent = null;
        rigidBody.AddForce(new Vector2(gameObject.transform.right.x, gameObject.transform.right.y) * Force, ForceMode2D.Force);
        //StartCoroutine(ExecuteWithDelay(() => rigidBody.IgnoreCollisions(GetComponentInParent<Weapon>().Rigidbody, false), 0.01f)); //пока не будет рикошетов - бесполезно
        //StartCoroutine(ExecuteWithDelay(() => rigidBody.IgnoreCollisions(GetComponentInParent<Character>().Rigidbody, false), 0.01f));
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Shell>() || collider.GetComponent<Bullet>() || collider.GetComponent<Weapon>()) return;
        //try { if (collision.gameObject.GetComponent<Bullet>() != null) return; } catch { }
        //try { if (collision.tag == "Player" || collision.transform.parent.tag == "Player") return; } catch { }
        Debug.Log(collider.gameObject.name);
        Destroy(gameObject);
    }
}

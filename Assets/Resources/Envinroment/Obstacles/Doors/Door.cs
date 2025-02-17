using UnityEngine;

public class Door : MonoBehaviour
{
    // HingeJoint2D joint;
    // Rigidbody2D rb;
    // Vector3 originalPos;
    // Vector3 rightPosition;
    // Vector3 leftPosition;

    // private void Start()
    // {
    //     joint = gameObject.GetComponent<HingeJoint2D>();
    //     rb = gameObject.GetComponent<Rigidbody2D>();
    //     originalPos = rb.position;
    // }

    // private void Update()
    // {
    //     gameObject.transform.position = originalPos;
    //     rightPosition = gameObject.transform.TransformPoint(gameObject.transform.localPosition + new Vector3(30, 3));
    //     leftPosition = gameObject.transform.TransformPoint(gameObject.transform.localPosition + new Vector3(30, -3));

    //     if (joint.jointAngle >= joint.limits.max - 5)
    //     {
    //         GameObject player = GetObject.GetNearest(gameObject, typeof(Player));
    //         if (Vector3.Distance(player.gameObject.transform.position, rightPosition) < Vector3.Distance(player.gameObject.transform.position, leftPosition))
    //         {
    //             rb.bodyType = RigidbodyType2D.Static;
    //         }
    //         else { rb.bodyType = RigidbodyType2D.Dynamic; }
    //         return;
    //     }
    //     if (joint.jointAngle <= joint.limits.min + 5)
    //     {
    //         GameObject player = GetObject.GetNearest(gameObject, typeof(Player));
    //         if (Vector3.Distance(player.gameObject.transform.position, rightPosition) > Vector3.Distance(player.gameObject.transform.position, leftPosition))
    //         {
    //             rb.bodyType = RigidbodyType2D.Static;
    //         }
    //         else { rb.bodyType = RigidbodyType2D.Dynamic; }
    //         return;
    //     }
    //     rb.bodyType = RigidbodyType2D.Dynamic;
    // }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Debug.Log("abobaEnter");
    //     //if (collision.rigidbody.bodyType == RigidbodyType2D.Static) { rb.bodyType = RigidbodyType2D.Static; }
    // }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     Debug.Log("abobaExit");
    // }
}
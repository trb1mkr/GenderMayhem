using Sirenix.OdinInspector;
using UnityEngine;

public class LastTouchedRigidbody : MonoBehaviour
{
    [SerializeField, ReadOnly] private Rigidbody2D _lastCollidedRigidbody;
    [SerializeField, ReadOnly] private Rigidbody2D _lastTriggeredRigidbody;

    public Rigidbody2D LastCollidedRigidbody => _lastCollidedRigidbody;
    public Rigidbody2D LastTriggeredRigidbody => _lastTriggeredRigidbody;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRigidbody = collision.collider.attachedRigidbody;
        //Debug.Log(otherRigidbody.name + " " + GetComponent<Rigidbody2D>().linearVelocity.magnitude);

        if (otherRigidbody != null)
            _lastCollidedRigidbody = otherRigidbody;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D otherRigidbody = other.attachedRigidbody;
        
        if (otherRigidbody != null)
            _lastTriggeredRigidbody = otherRigidbody;
    }
}
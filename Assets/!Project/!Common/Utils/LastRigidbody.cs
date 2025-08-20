using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class LastRigidbody : MonoBehaviour
{
    [SerializeField] bool _forget = false;
    [ShowIf("_forget")][SerializeField] float _forgetDelay;

    [SerializeField, ReadOnly] private Rigidbody2D _lastCollidedRigidbody;
    [SerializeField, ReadOnly] private Rigidbody2D _lastTriggeredRigidbody;

    public Rigidbody2D LastCollidedRigidbody => _lastCollidedRigidbody;
    public Rigidbody2D LastTriggeredRigidbody => _lastTriggeredRigidbody;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRigidbody = collision.collider.attachedRigidbody;
        //Debug.Log(otherRigidbody.name + " " + GetComponent<Rigidbody2D>().linearVelocity.magnitude);

        if (otherRigidbody != null)
        {
            _lastCollidedRigidbody = otherRigidbody;
            if (otherRigidbody == _lastCollidedRigidbody)
            {
                StopCoroutine(ForgetCollided());
                return;
            }
            if (_forget) StartCoroutine(ForgetCollided());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D otherRigidbody = other.attachedRigidbody;

        if (otherRigidbody != null)
        {
            _lastTriggeredRigidbody = otherRigidbody;
            if (otherRigidbody == _lastCollidedRigidbody)
            {
                StopCoroutine(ForgetTriggered());
                return;
            }
            if (_forget) StartCoroutine(ForgetTriggered());
        }
    }

    private IEnumerator ForgetCollided()
    {
        yield return new WaitForSeconds(_forgetDelay);
        _lastCollidedRigidbody = null;
        Debug.Log("forgot");
    }
    
    private IEnumerator ForgetTriggered()
    {
        yield return new WaitForSeconds(_forgetDelay);
        _lastCollidedRigidbody = null;
    }
}
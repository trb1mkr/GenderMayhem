using UnityEngine;

public class Door : MonoBehaviour
{
    private Rigidbody2D _rb;
    private LastRigidbody _lastrb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lastrb = GetComponent<LastRigidbody>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponentInParent<Enemy>();
        if (enemy && _rb.linearVelocity.magnitude * _rb.mass > enemy.Health.KnockdownForce &&
            _lastrb.LastCollidedRigidbody != null && _lastrb.LastCollidedRigidbody.GetComponentInParent<Character>() != enemy &&
            !_lastrb.LastCollidedRigidbody.GetComponentInParent<Character>().Health.IsUnconscious)
            enemy.Health.FallUnconscious(collision.contacts[0].point);
        
        // //Debug.Log(_rb.linearVelocity.magnitude * _rb.mass);
        // Character character = collision.collider.GetComponentInParent<Character>();
        // //Debug.Log(character);
        // //Debug.Log(_lastrb.LastCollidedRigidbody);
        // if (character && _rb.linearVelocity.magnitude * _rb.mass > character.Health.KnockdownForce &&
        //     _lastrb.LastCollidedRigidbody != null && _lastrb.LastCollidedRigidbody.GetComponentInParent<Character>() != character &&
        //     !_lastrb.LastCollidedRigidbody.GetComponentInParent<Character>().Health.IsUnconscious)
        //     character.Health.FallUnconscious(collision.contacts[0].point);
    }
}
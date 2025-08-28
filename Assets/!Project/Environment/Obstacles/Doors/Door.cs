using UnityEngine;

public class Door : MonoBehaviour
{
    #region Values
    [SerializeField] private AudioClip _punchSound, _creakSound;
    [SerializeField] private float _creakVelocity, _creakChance;
    #endregion

    #region References
    private Rigidbody2D _rb;
    private LastRigidbody _lastrb;
    private AudioSource _audioSource;
    #endregion

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lastrb = GetComponent<LastRigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        KnockEnemy(collision);
        Creak(collision);
    }

    private void KnockEnemy(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponentInParent<Enemy>();
        if (enemy && _rb.linearVelocity.magnitude * _rb.mass > enemy.Health.KnockdownForce &&
            _lastrb.LastCollidedRigidbody != null && _lastrb.LastCollidedRigidbody.GetComponentInParent<Character>() != enemy &&
            !_lastrb.LastCollidedRigidbody.GetComponentInParent<Character>().Health.IsUnconscious)
        {
            enemy.Health.FallUnconscious(collision.contacts[0].point);
            _audioSource.PlayOneShot(_punchSound);
        }
    }
    
    private void Creak(Collision2D collision)
    {
        Character character = collision.collider.GetComponentInParent<Character>();
        if (character && _rb.linearVelocity.magnitude > _creakVelocity && _creakChance > Random.value)
            _audioSource.PlayOneShot(_creakSound);
    }

    // //Debug.Log(_rb.linearVelocity.magnitude * _rb.mass);
    // Character character = collision.collider.GetComponentInParent<Character>();
    // //Debug.Log(character);
    // //Debug.Log(_lastrb.LastCollidedRigidbody);
    // if (character && _rb.linearVelocity.magnitude * _rb.mass > character.Health.KnockdownForce &&
    //     _lastrb.LastCollidedRigidbody != null && _lastrb.LastCollidedRigidbody.GetComponentInParent<Character>() != character &&
    //     !_lastrb.LastCollidedRigidbody.GetComponentInParent<Character>().Health.IsUnconscious)
    //     character.Health.FallUnconscious(collision.contacts[0].point);
}
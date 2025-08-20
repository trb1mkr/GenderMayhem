using GenderMayhem.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    #region Values
    [ReadOnly] public ActionEventsGroup ActionEventsGroup;
    [SerializeField] private float _sleepVelocity = 0.1f;
    [SerializeField] private float _ignoreVelocity = 30f;
    #endregion

    #region References
    [HideInInspector] public Rigidbody2D Rigidbody;
    [HideInInspector] public PolygonCollider2D Collider;
    [HideInInspector] public AudioSource AudioSource;
    [HideInInspector] public AudioSourceEmitter AudioSourceEmitter;
    [HideInInspector] public SpriteRenderer SpriteRenderer;

    public AudioClip PickUpSound, ThrowSound;
    #endregion

    public virtual void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<PolygonCollider2D>();
        AudioSourceEmitter = GetComponent<AudioSourceEmitter>();
    }

    void Update()
    {
        if (Rigidbody.linearVelocity.magnitude <= _ignoreVelocity)
        {
            if (Rigidbody.linearVelocity.magnitude <= _sleepVelocity)
                Rigidbody.simulated = false;
            gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        }
        else gameObject.layer = LayerMask.NameToLayer("Items");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.name);
        var character = collision.collider.GetComponentInParent<Character>();
        if (character != null && collision.collider == character.StateController.TorsoCollider)
        {
            Vector2 collisionVelocity = Rigidbody.linearVelocity; //- character.Rigidbody.linearVelocity;
            var lastTouched = GetComponent<LastRigidbody>();
            if (collisionVelocity.magnitude * Rigidbody.mass > character.Health.KnockdownForce) //lastTouched.LastCollidedRigidbody != character.Rigidbody && 
                character.Health.FallUnconscious(collision.contacts[0].point);
        }
    }

    public virtual void OnPickUp()
    {

    }

    public virtual void OnThrow()
    {

    }
}

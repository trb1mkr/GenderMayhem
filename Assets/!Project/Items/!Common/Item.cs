using GenderMayhem.Actions;
using Sirenix.OdinInspector;
using System.Collections;
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
    [HideInInspector] public SpriteRenderer SpriteRenderer;

    public AudioClip PickUpSound, ThrowSound;
    #endregion

    public virtual void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<PolygonCollider2D>();
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
        Debug.Log(collision.collider.name);
        var character = collision.collider.GetComponentInParent<Character>();
        if (character != null && collision.collider == character.StateController.BodyCollider)
        {
            Vector2 collisionVelocity = Rigidbody.linearVelocity; //- character.Rigidbody.linearVelocity;
            var lastTouched = GetComponent<LastTouchedRigidbody>();
            if (lastTouched.LastCollidedRigidbody != character.Rigidbody && collisionVelocity.magnitude * Rigidbody.mass > character.Health.KnockdownForce)
                character.Health.Knockdown();
        }
    }
}

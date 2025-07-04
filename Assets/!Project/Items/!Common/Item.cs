using GenderMayhem.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    #region Values
    [ReadOnly] public ActionEventsGroup ActionEventsGroup;
    float _sleepVelocity = 0.01f;
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
        if (Rigidbody.linearVelocity.magnitude <= _sleepVelocity) 
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
            Rigidbody.simulated = false;
        }
        else gameObject.layer = LayerMask.NameToLayer("Items");
    }
}

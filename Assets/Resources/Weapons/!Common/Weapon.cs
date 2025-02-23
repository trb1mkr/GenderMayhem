using UnityEngine;

[System.Serializable]
abstract public class Weapon : MonoBehaviour
{
    #region Values
    [SerializeField] float _sleepVelocity = 1f;
    public AudioClip AttackSound;
    public AudioClip PickUpSound;
    #endregion

    #region References
    [HideInInspector] public Rigidbody2D Rigidbody;
    [HideInInspector] public PolygonCollider2D Collider;
    [HideInInspector] public AudioSource AudioSource;
    [HideInInspector] public SpriteRenderer SpriteRenderer;
    #endregion

    public void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Collider = GetComponent<PolygonCollider2D>();
    }

    virtual public void Attack() { GetComponent<CameraShakeSource>().Shake(); }

    virtual public void AltAttack() { return; }

    void Update()
    {
        if (Rigidbody.linearVelocity.magnitude <= _sleepVelocity) 
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore");
            Rigidbody.simulated = false;
        }
        else gameObject.layer = LayerMask.NameToLayer("Collide");
    }
}
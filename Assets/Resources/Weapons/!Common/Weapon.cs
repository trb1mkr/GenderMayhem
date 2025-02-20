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
    [HideInInspector] public AudioSource AudioSource;
    [HideInInspector] public SpriteRenderer SpriteRenderer;
    #endregion

    public void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    virtual public void Attack() { return; }

    void Update()
    {
        if (Rigidbody.linearVelocity.magnitude <= _sleepVelocity) 
        {
            gameObject.layer = LayerMask.NameToLayer("Weapons");
            Rigidbody.simulated = false;
        }
    }

    public void OnPickUp()
    {
        SpriteRenderer.enabled = false;
        Rigidbody.simulated = false;
        AudioSource.PlayOneShot(PickUpSound);
    }

    public void OnThrow()
    {
        SpriteRenderer.enabled = true;
        //Physics2D.IgnoreCollision()
        gameObject.layer = LayerMask.NameToLayer("Enemies");
        Rigidbody.simulated = true;
        AudioSource.PlayOneShot(AttackSound);
    }
}
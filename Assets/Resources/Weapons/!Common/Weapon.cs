using UnityEngine;

[System.Serializable]
abstract public class Weapon : MonoBehaviour
{
    [HideInInspector] public AudioSource AudioSource;
    [HideInInspector] public Rigidbody2D Rigidbody;
    public AudioClip AttackSound;
    public AudioClip PickUpSound;

    public void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    virtual public void Attack() { return; }

    void Update()
    {
        if (Rigidbody.simulated == true)
            transform.rotation = Quaternion.Euler(0, 0, Rigidbody.rotation);

        ProgressiveDrag();

        if (Rigidbody.linearVelocity.x + Rigidbody.linearVelocity.y == 0) gameObject.layer = 6;
        else gameObject.layer = 8;
    }

    void ProgressiveDrag()
    {
        if (GetComponent<SpriteRenderer>().enabled == true && 
            (Mathf.Abs(Rigidbody.linearVelocity.x) + Mathf.Abs(Rigidbody.linearVelocity.y)) < 100 && 
            (Mathf.Abs(Rigidbody.linearVelocity.x) + Mathf.Abs(Rigidbody.linearVelocity.y)) > 1)
        {
            Rigidbody.linearDamping = Rigidbody.linearDamping + 0.05f;
            Rigidbody.angularDamping = Rigidbody.angularDamping + 0.05f;
        }
        else
        {
            Rigidbody.linearDamping = 2;
            Rigidbody.angularDamping = 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<SpriteRenderer>().enabled == true && (Mathf.Abs(Rigidbody.linearVelocity.x) + Mathf.Abs(Rigidbody.linearVelocity.y)) > 20)
        {
            try { collision.gameObject.GetComponent<Glass>().BreakGlass(); }
            catch { }
        }
    }
}
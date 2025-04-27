using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    #region Values
    //public float UsageTime;
    float _sleepVelocity = 0.01f;
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
    
    // public virtual IEnumerator Use()
    //     { yield return new WaitForSeconds(UsageTime); }

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

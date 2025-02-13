using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
abstract public class Weapon : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public Rigidbody2D rb;
    public AudioClip attackSound;
    public AudioClip pickUpSound;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.simulated == true)
        {
            transform.rotation = Quaternion.Euler(0, 0, rb.rotation);
        }
        ProgressiveDrag();
        if (rb.linearVelocity.x + rb.linearVelocity.y == 0) gameObject.layer = 6;
        else gameObject.layer = 8;
    }

    void ProgressiveDrag()
    {
        if (GetComponent<SpriteRenderer>().enabled == true && (Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.y)) < 100 && (Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.y)) > 1)
        {
            rb.linearDamping = rb.linearDamping + 0.05f;
            rb.angularDamping = rb.angularDamping + 0.05f;
        }
        else
        {
            rb.linearDamping = 2;
            rb.angularDamping = 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<SpriteRenderer>().enabled == true && (Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.y)) > 20)
        {
            try { collision.gameObject.GetComponent<Glass>().BreakGlass(); }
            catch { }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    public Sprite brokenGlass;
    public AudioClip glassBreak;
    public AudioClip glassWalk;

    public void BreakGlass()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == brokenGlass) return;
        spriteRenderer.sprite = brokenGlass;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, 16);
        GetComponent<AudioSource>().PlayOneShot(glassBreak);
        GetComponent<BoxCollider2D>().isTrigger = true;
        //Destroy(GetComponent<BoxCollider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<AudioSource>().PlayOneShot(glassWalk);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}

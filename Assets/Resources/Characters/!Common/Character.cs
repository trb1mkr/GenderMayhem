using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public string state;
    [HideInInspector] public string currentAnimationFrame;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public PolygonCollider2D[] polygonColliders;
    [HideInInspector] public Transform weaponPoint;

    public List<AnimationFrame> animationFrames = new List<AnimationFrame>();

    public Weapon weapon; //HideInInspector ме мсфем
    public Weapon fists;

    public void SetDefaultComponents()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        weaponPoint = gameObject.transform.GetChild(0).transform;
        polygonColliders[0] = gameObject.GetComponent<PolygonCollider2D>();
        polygonColliders[1] = gameObject.transform.GetChild(1).GetComponent<PolygonCollider2D>();
        polygonColliders[2] = gameObject.transform.GetChild(2).GetComponent<PolygonCollider2D>();
    }

    public void Animate()
    {
        string weaponName = weapon.GetType().Name;
        if (weaponName == null) weaponName = "";
        string animationName = weaponName + state;
        //Debug.Log(animationName);
        var animationFrame = animationFrames.Find(x => x.name == animationName);
        if (animationFrame == null) { Debug.Log("No Such Animation"); return; }
        if (spriteRenderer.sprite == animationFrame.sprite) return;
        currentAnimationFrame = animationName;
        spriteRenderer.sprite = animationFrame.sprite;
        weaponPoint.localPosition = AnimationFrame.LoadTransform(animationFrame.weaponPoint)[0];
        weaponPoint.localRotation = Quaternion.Euler(AnimationFrame.LoadTransform(animationFrame.weaponPoint)[1]);
        polygonColliders[0].SetPath(0, AnimationFrame.LoadPath(animationFrame.torsoCollider));
        polygonColliders[1].SetPath(0, AnimationFrame.LoadPath(animationFrame.attackCollider));
        polygonColliders[2].SetPath(0, AnimationFrame.LoadPath(animationFrame.avoidCollider));
    }

    public void ThrowWeapon(Vector2 direction)
    {
        Debug.DrawLine(gameObject.transform.position, direction, Color.red, 5f);
        weapon.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        weapon.gameObject.transform.SetParent(null, true);
        weapon.GetComponent<Rigidbody2D>().AddForce((new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) - (direction)).normalized* -1 * 10000, ForceMode2D.Impulse);
        weapon.GetComponent<Rigidbody2D>().AddTorque(10000, ForceMode2D.Impulse);
        weapon.GetComponent<SpriteRenderer>().enabled = true;
        weapon = fists;
    }

    public void Dead()
    {
        state = "Dead";
        ThrowWeapon(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    }

    public virtual void Unconscious()
    {
        state = "Unconscious";
        ThrowWeapon(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    }
}
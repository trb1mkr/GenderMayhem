using Sirenix.OdinInspector;
using UnityEngine;

public class Glass : MonoBehaviour
{
    #region Values
    public bool IsBroken;
    public Sprite BrokenGlass;
    public AudioClip GlassBreak;
    public AudioClip GlassWalk;
    [MinValue(0)][MaxValue(1)] public float GlassWalkPlayChance;
    #endregion

    public void BreakGlass()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == BrokenGlass) return;
        spriteRenderer.sprite = BrokenGlass;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, 16);
        GetComponent<AudioSource>().PlayOneShot(GlassBreak);
        GetComponent<BoxCollider2D>().isTrigger = true;
        IsBroken = true;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!IsBroken) return;
        if (Random.value > GlassWalkPlayChance || !collider.attachedRigidbody.TryGetComponent(out Character _)) return;
        if (collider.attachedRigidbody.linearVelocity.magnitude == 0) return; // потом если не будет работать для ии, то сделать через is
        
        GetComponent<AudioSource>().PlayOneShot(GlassWalk);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Weapon weapon))
            if (weapon.Rigidbody.linearVelocity.magnitude > 20)
                BreakGlass();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Bullet>())
            BreakGlass();

        Player player = collider.GetComponentInParent<Player>();
        if (player)
            if (player.AttackCollider == collider && player.StateId == CharacterStateId.Attack)
                BreakGlass();
    }
}

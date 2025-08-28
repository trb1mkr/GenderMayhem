using Sirenix.OdinInspector;
using UnityEngine;

public class Glass : MonoBehaviour
{
    #region Values
    public bool IsBroken;
    public Sprite Broken;
    public AudioClip Break;
    public AudioClip Walk;
    [MinValue(0)][MaxValue(1)] public float GlassWalkPlayChance;
    #endregion

    public void BreakGlass()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == Broken) return;
        spriteRenderer.sprite = Broken;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, 16);
        GetComponent<AudioSource>().PlayOneShot(Break);
        GetComponent<BoxCollider2D>().isTrigger = true;
        IsBroken = true;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!IsBroken) return;
        if (Random.value > GlassWalkPlayChance || collider.GetComponent<Character>() == null || (collider.GetComponent<Character>() && collider.GetComponent<Character>().StateController.TorsoCollider != collider)) return;
        if (collider.attachedRigidbody.linearVelocity.magnitude == 0) return; // потом если не будет работать для ии, то сделать через is
        
        GetComponent<AudioSource>().PlayOneShot(Walk);
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
            if (player.StateController.AttackCollider == collider && player.StateController.StateId == CharacterStateId.Attack)
                BreakGlass();
    }
}

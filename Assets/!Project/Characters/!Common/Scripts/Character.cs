using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Character : MonoBehaviour
{
    #region Values
    public CharacterStateId StateId;
    [ReadOnly] public bool Dead;
    [ReadOnly] public bool Unconscious;
    #endregion

    #region References
    public SpriteRenderer SpriteRenderer { get; protected set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public AudioSource AudioSource { get; protected set; }
    
    [field: SerializeField] public PolygonCollider2D BodyCollider { get; protected set; }
    [field: SerializeField] public PolygonCollider2D AttackCollider { get; protected set; }
    [field: SerializeField] public PolygonCollider2D AvoidCollider { get; protected set; }
    [field: SerializeField] public Transform WeaponPoint { get; protected set; }

    public CharacterAnimator Animate { get; private set; }
    public CharacterItemManager ItemManager { get; private set; }
    #endregion

    public void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();

        Animate = GetComponent<CharacterAnimator>();
        ItemManager = GetComponent<CharacterItemManager>();
        
        Animate.Character = ItemManager.Character = this;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider == BodyCollider)
        {
            //Debug.Log("BodyCollider");
            //if (collision.collider.GetComponent<Weapon>()) FallUnconscious();
            if (collision.collider.GetComponent<Bullet>()) Die();
        }
    }

    public void Die()
    {
        Dead = true;
        ItemManager.Throw();
        //Throw(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    }

    public void FallUnconscious()
    {
        Unconscious = true;
        ItemManager.Throw();
        Rigidbody.simulated = false;
        gameObject.layer = LayerMask.NameToLayer("Ignore");
        //Throw(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    }
}
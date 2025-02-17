using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; protected set; }
    public Rigidbody2D Rigidbody { get; private set; }
    public AudioSource AudioSource { get; protected set; }
    public PolygonCollider2D BodyCollider { get; protected set; }
    public PolygonCollider2D AttackCollider { get; protected set; } //отключать для ганов, если нет удара прикладом
    public PolygonCollider2D AvoidCollider { get; protected set; } //отключать для мили
    public Transform WeaponPoint { get; protected set; }

    public CharacterAnimate Animate { get; private set; }
    public CharacterItems Items { get; private set; }

    public string State;
    public Weapon Weapon;
    public Weapon Fists;

    public void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        //WeaponPoint = transform.GetChild(0).transform;
        Rigidbody = GetComponent<Rigidbody2D>();
        Debug.Log(Rigidbody.gameObject.name);
    }

    // public void Dead()
    // {
    //     State = "Dead";
    //     Throw(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    // }

    // public virtual void Unconscious()
    // {
    //     State = "Unconscious";
    //     Throw(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    // }
}
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
    [field: SerializeField] public PolygonCollider2D AttackCollider { get; protected set; } //отключать для ганов, если нет удара прикладом
    [field: SerializeField] public PolygonCollider2D AvoidCollider { get; protected set; } //отключать для мили
    [field: SerializeField] public Transform WeaponPoint { get; protected set; }

    public CharacterAnimate Animate { get; private set; }
    public CharacterHands Hands { get; private set; }
    #endregion

    public void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();

        Animate = GetComponent<CharacterAnimate>();
        Hands = GetComponent<CharacterHands>();
        
        Animate.Character = Hands.Character = this;
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
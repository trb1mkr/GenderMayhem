using UnityEngine;

public abstract class Character : MonoBehaviour
{
    #region References
    public Rigidbody2D Rigidbody { get; protected set; }
    public AudioSource AudioSource { get; protected set; }
    public Animator Animator { get; protected set; }

    public CharacterAnimatorController AnimatorController { get; protected set; }
    public CharacterItemManager ItemManager { get; protected set; }
    public CharacterStateController StateController { get; protected set; }
    public CharacterHealth Health { get; protected set; }
    public CharacterWeaponController WeaponController { get; protected set; }
    public ColliderTriggerTagFilter AvoidList { get; protected set; }
    public CharacterMeleeAttack MeleeAttack { get; protected set; }
    public CharacterBody Body { get; protected set; }
    #endregion

    public void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();
        Animator = GetComponent<Animator>();

        AnimatorController = GetComponent<CharacterAnimatorController>();
        ItemManager = GetComponent<CharacterItemManager>();
        StateController = GetComponent<CharacterStateController>();
        Health = GetComponent<CharacterHealth>();
        WeaponController = GetComponent<CharacterWeaponController>();
        AvoidList = GetComponentInChildren<ColliderTriggerTagFilter>();
        MeleeAttack = GetComponentInChildren<CharacterMeleeAttack>();
        Body = GetComponent<CharacterBody>();

        AnimatorController.Character = ItemManager.Character = StateController.Character = WeaponController.Character = MeleeAttack.Character = Body.Character = this;
    }
}
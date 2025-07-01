using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections;

public class CharacterItemManager : MonoBehaviour
{
    #region Values
    [field: ReadOnly] public WeaponId WeaponId { get; private set; }
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _throwTorque;
    public Coroutine UseRoutine;
    #endregion

    #region References
    [HideInInspector] public Character Character;
    [field: SerializeField] public Weapon Weapon { get; private set; }
    [SerializeField] private Weapon _fists;
    #endregion

    void Awake()
    {
        SetWeaponId();
    }

    void SetWeaponId()
    {
        if (Enum.TryParse(Weapon.GetType().Name, out WeaponId weaponId))
            WeaponId = weaponId;
        else
            throw new ArgumentException($"Weapon type '{Weapon.GetType().Name}' not found in WeaponId enum.");
    }

    void Update()
    {
        //Debug.Log(Physics2D.GetIgnoreCollision(Character.BodyCollider, Weapon.Collider));
    }

    public GameObject GetTargetWeapon()
    {
        // GameObject weaponUnderCursor = GetWeaponUnderCursor();

        // if (weaponUnderCursor != null)
        //     return weaponUnderCursor;

        return GetNearestWeapon();
    }

    // private GameObject GetWeaponUnderCursor()
    // {
    //     // Получаем позицию курсора мыши в мировых координатах
    //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Character.Controls.MousePosition);
    //     mousePosition.z = 0; // Обнуляем Z, так как мы работаем в 2D

    //     // Используем Raycast для проверки, наведён ли курсор на объект
    //     RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

    //     if (hit.collider != null)
    //     {
    //         // Проверяем, есть ли у объекта компонент Weapon
    //         Weapon weapon = hit.collider.GetComponent<Weapon>();
    //         if (weapon != null)
    //         {
    //             return weapon.gameObject; // Возвращаем объект, на который наведён курсор
    //         }
    //     }

    //     return null; // Если ничего не найдено
    // }

    private GameObject GetNearestWeapon()
    {
        GameObject nearestWeapon = null;
        float nearestDistance = float.MaxValue;

        // Получаем все объекты с компонентом Weapon
        Weapon[] weapons = FindObjectsByType<Weapon>(FindObjectsSortMode.None);

        foreach (Weapon weapon in weapons)
        {
            if (weapon.transform.GetComponentInParent<Player>() != null) continue;
            // Вычисляем дистанцию между игроком и оружием
            float distance = Vector3.Distance(transform.position, weapon.transform.position);

            // Проверяем, что дистанция меньше максимальной и меньше текущей ближайшей дистанции
            if (distance <= _pickUpDistance && distance < nearestDistance)
            {
                nearestWeapon = weapon.gameObject;
                nearestDistance = distance;
            }
        }

        return nearestWeapon;
    }

    // public void Use() //сделать предметы, которые используются моментально и которые требует время на использование
    // {
    //     if (Weapon is Melee melee)
    //     {
    //         if (Character.StateId == CharacterStateId.Idle && !melee.IsCooldown)
    //         {
    //             Character.StateId = CharacterStateId.Attack;
    //             Weapon.Attack();
    //         }
    //     }
    //     else Weapon.Attack();
    // }

    public void AltUse()
    {
        if (Weapon is Gun)
            if (Character.StateId == CharacterStateId.Idle)
            {
                Character.StateId = CharacterStateId.Attack;
                Weapon.AltAttack();
            }
    }

    public void Use()
    {
        if (Weapon is Melee melee)
            if (!melee.IsCooldown) //Character.StateId == CharacterStateId.Idle && 
            {
                Character.StateId = CharacterStateId.Attack;
                UseRoutine = StartCoroutine(Weapon.Use());
                return;
            }
        if (Weapon is Gun) UseRoutine = StartCoroutine(Weapon.Use());
    }

    public void PickUp()
    {
        if (Character.StateId == CharacterStateId.Attack) return;

        GameObject targetWeapon = GetTargetWeapon();
        if (Weapon != _fists) Throw();
        if (targetWeapon == null) return;

        Weapon = targetWeapon.GetComponent<Weapon>();
        Weapon.SpriteRenderer.enabled = false;
        Weapon.Rigidbody.IgnoreCollisions(Character.Rigidbody, true);
        Weapon.Rigidbody.simulated = false;
        Weapon.transform.parent = Character.WeaponPoint;
        Weapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Weapon.AudioSource.PlayOneShot(Weapon.PickUpSound);

        SetWeaponId();
    }

    public void Throw()
    {
        if (Character.StateId == CharacterStateId.Attack) return;
        if (Weapon == _fists) return;

        Character.StateId = CharacterStateId.Idle;
        
        Weapon.SpriteRenderer.enabled = true;
        this.Invoke(() => Weapon.Rigidbody.IgnoreCollisions(Character.Rigidbody, false), 0.1f);
        Weapon.Rigidbody.simulated = true;
        Weapon.transform.SetParent(null, true);
        Weapon.Rigidbody.AddForce(_throwForce * new Vector2(transform.right.x, transform.right.y).normalized, ForceMode2D.Impulse);
        Weapon.Rigidbody.AddTorque(_throwTorque, ForceMode2D.Impulse);
        Weapon.AudioSource.PlayOneShot(Weapon.AttackSound);
        Weapon = _fists;

        SetWeaponId();
    }
}

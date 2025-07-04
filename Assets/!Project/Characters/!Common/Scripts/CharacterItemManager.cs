using UnityEngine;
using UnityEngine.Events;

public class CharacterItemManager : MonoBehaviour
{
    #region Values
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _throwTorque;
    public Coroutine UseRoutine;
    #endregion

    #region References
    [HideInInspector] public Character Character;
    public Item Item;
    [SerializeField] private Weapon _fists;
    [HideInInspector] public UnityEvent OnItemChange;
    #endregion

    public GameObject GetTargetItem()
    {
        return GetNearestItem();
    }

    private GameObject GetNearestItem()
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

    public void PickUp()
    {
        if (Character.StateController.StateId == CharacterStateId.Attack) return;

        GameObject targetItem = GetTargetItem();
        if (Item != _fists) Throw();
        if (targetItem == null) return;

        Item = targetItem.GetComponent<Item>();
        Item.SpriteRenderer.enabled = false;
        Item.Rigidbody.IgnoreCollisions(Character.Rigidbody, true);
        Item.Rigidbody.simulated = false;
        Item.transform.parent = Character.WeaponPoint;
        Item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Item.AudioSource.PlayOneShot(Item.PickUpSound);

        OnItemChange.Invoke();
    }

    public void Throw()
    {
        if (Character.StateController.StateId == CharacterStateId.Attack) return;
        if (Item == _fists) return;

        Character.StateController.StateId = CharacterStateId.Idle;

        Item.SpriteRenderer.enabled = true;
        this.Invoke(() => Item.Rigidbody.IgnoreCollisions(Character.Rigidbody, false), 0.1f);
        Item.Rigidbody.simulated = true;
        Item.transform.SetParent(null, true);
        Item.Rigidbody.AddForce(_throwForce * new Vector2(transform.right.x, transform.right.y).normalized, ForceMode2D.Impulse);
        Item.Rigidbody.AddTorque(_throwTorque, ForceMode2D.Impulse);
        Item.AudioSource.PlayOneShot(Item.ThrowSound);
        Item = _fists;

        OnItemChange.Invoke();
    }
}

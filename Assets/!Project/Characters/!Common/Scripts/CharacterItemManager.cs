using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;

public class CharacterItemManager : MonoBehaviour
{
    #region Values
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _throwTorque;
    public Coroutine UseRoutine;
    public Action ItemChanged;
    public event Action ItemPickedUp;
    public event Action ItemThrowed;
    private Coroutine _characterIgnoreCoroutine;
    #endregion

    #region References
    [HideInInspector] public Character Character;
    public Item Item;
    [SerializeField] private Weapon _fists;
    [OnValueChanged("SetUpStarterItem"), SerializeField] private GameObject _starterItem; 
    #endregion

    void Awake()
    {
        if (!Item) Item = _fists;
    }

    void Start()
    {
        ItemChanged?.Invoke();
        ItemPickedUp?.Invoke();
    }

    public GameObject GetTargetItem()
    {
        return GetNearestItem();
    }

    private GameObject GetNearestItem()
    {
        GameObject nearestWeapon = null;
        float nearestDistance = float.MaxValue;

        Weapon[] weapons = FindObjectsByType<Weapon>(FindObjectsSortMode.None);
        foreach (Weapon weapon in weapons)
        {
            if (weapon.transform.GetComponentInParent<Character>() != null) continue;
            if (Physics2D.Linecast(weapon.transform.GetComponent<Item>().Collider.ClosestPoint(Character.transform.position), Character.transform.position, LayerMask.GetMask("Obstacles")).collider != null) continue;

            float distance = Vector3.Distance(transform.position, weapon.transform.position);
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

        if (_characterIgnoreCoroutine != null) StopCoroutine(_characterIgnoreCoroutine);
        Item = targetItem.GetComponent<Item>();
        Item.SpriteRenderer.enabled = false;
        Item.Rigidbody.IgnoreCollisions(Character.Rigidbody, true);
        Item.Rigidbody.simulated = false;
        Item.transform.parent = Character.StateController.WeaponPoint;
        Item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Item.AudioSource.PlayOneShot(Item.PickUpSound);

        ItemChanged?.Invoke();
        ItemPickedUp?.Invoke();
    }

    public void Throw()
    {
        if (Character.StateController.StateId == CharacterStateId.Attack) return;
        if (Item == _fists) return;

        ItemThrowed.Invoke();

        Item.SpriteRenderer.enabled = true;
        _characterIgnoreCoroutine = StartCoroutine(UnIgnoreCharacter(Item));
        Item.Rigidbody.simulated = true;
        Item.transform.SetParent(null, true);
        Item.Rigidbody.AddForce(_throwForce * new Vector2(transform.right.x, transform.right.y).normalized, ForceMode2D.Impulse);
        Item.Rigidbody.AddTorque(_throwTorque, ForceMode2D.Impulse);
        Item.AudioSource.PlayOneShot(Item.ThrowSound);
        Item = _fists;

        ItemChanged?.Invoke();
        ItemThrowed?.Invoke();
    }

    private IEnumerator UnIgnoreCharacter(Item item)
    {
        yield return new WaitForSeconds(0.5f);
        item.Rigidbody.IgnoreCollisions(Character.Rigidbody, false);
    }

    private void SetUpStarterItem()
    {
        if (_starterItem == null) Item = _fists;
        if (GetComponent<CharacterStateController>().WeaponPoint.childCount != 0) DestroyImmediate(GetComponent<CharacterStateController>().WeaponPoint.GetChild(0).gameObject);

        if (_starterItem != null)
        Item = Instantiate(_starterItem, transform.position, transform.rotation, GetComponent<CharacterStateController>().WeaponPoint).GetComponent<Item>();
        Item.GetComponent<SpriteRenderer>().enabled = false;
        Item.GetComponent<Rigidbody2D>().IgnoreCollisions(Item.transform.parent.GetComponentInParent<Rigidbody2D>(), true);
        Item.GetComponent<Rigidbody2D>().simulated = false;
    }
}

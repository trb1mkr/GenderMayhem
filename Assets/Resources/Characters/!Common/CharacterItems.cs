using UnityEngine;

public class CharacterItems : MonoBehaviour
{
    [SerializeField] private float _pickUpDistance;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _throwTorque;

    #region References
    [HideInInspector] public Character Character;
    #endregion

    public GameObject GetTargetWeapon()
    {
        //получать тот weapon на который навёлся

        // var nearestweapon = GetObject.GetNearest(gameObject, "Weapon");

        // if (nearestweapon != null)
        //     if (Vector2.Distance(nearestweapon.transform.position, gameObject.transform.position) < 15)
        //         return nearestweapon;

        return null;
    }

    public void PickUp()
    {
        if (Character.Weapon != null) Throw();
        GameObject targetWeapon = GetTargetWeapon();

        targetWeapon.GetComponent<SpriteRenderer>().enabled = false;
        targetWeapon.GetComponent<Rigidbody2D>().simulated = false;
        targetWeapon.gameObject.transform.parent = Character.WeaponPoint;
        targetWeapon.gameObject.transform.localPosition = Vector3.zero;
        targetWeapon.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Character.Weapon = targetWeapon.GetComponent<Weapon>();
        Character.AudioSource.PlayOneShot(Character.Weapon.PickUpSound);
    }

    public void Throw()
    {
        Character.AudioSource.PlayOneShot(Character.Weapon.AttackSound);
        StopCoroutine("Melee");
        Character.State = "Idle";

        var direction = new Vector2(transform.up.x, transform.up.y);
        Debug.DrawLine(gameObject.transform.position, direction, Color.red, 5f);
        Character.Weapon.Rigidbody.simulated = true;
        Character.Weapon.transform.SetParent(null, true);
        Character.Weapon.Rigidbody.AddForce(_throwForce * -1 * (new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) - direction).normalized, ForceMode2D.Impulse);
        Character.Weapon.Rigidbody.AddTorque(_throwTorque, ForceMode2D.Impulse);
        Character.Weapon.GetComponent<SpriteRenderer>().enabled = true;
        Character.Weapon = Character.Fists;
    }

    // void ThrowPickUp()
    // {
    //     if (State == "Dead" || State == "Unconscious") return;
    //     if (Weapon.GetType() == typeof(Fists))
    //     {
    //         var nearestWeapon = GetNearestWeapon();
    //         if (nearestWeapon) { PickUp(nearestWeapon); }
    //     }
    //     else
    //     {
    //         if (GetNearestWeapon() == null)
    //         {
    //             Throw();
    //         }
    //         else
    //         {
    //             Swap();
    //         }
    //     }
    // }
}

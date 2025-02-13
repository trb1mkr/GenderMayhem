using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    Rigidbody2D rb;
    Camera cam;

    public int movementSpeed;
    Vector2 movementDirection;

    Vector2 mousePosition;
    Vector2 worldMousePosition;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        SetDefaultComponents();
        state = "Idle";
        weapon = fists;
    }

    void Update()
    {
        Animate();
    }

    void FixedUpdate() 
    {
        if (state == "Dead" || state == "Unconscious") return;
        if (cam == null) { cam = Camera.main; }
        Move();
        Rotate();
        CheckObstacles();
    }

    #region Movement
    void OnMove(InputValue movement)
    {
        movementDirection = movement.Get<Vector2>();
    }

    public void Move()
    {
        //rb.velocity = new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed);
        rb.AddForce(new Vector2(movementDirection.x * movementSpeed, movementDirection.y * movementSpeed) * 1000f, ForceMode2D.Force);
    }

    void Rotate()
    {
        var targetPosition = cam.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y - cam.transform.position.z)); //TRANSFORM.LOOKAT ���������
        rb.rotation = Mathf.Atan2((targetPosition.y - transform.position.y), (targetPosition.x - transform.position.x)) * Mathf.Rad2Deg; //.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((targetPosition.y - transform.position.y), (targetPosition.x - transform.position.x)) * Mathf.Rad2Deg);
    }
    #endregion

    #region Actions
    void OnThrowPickUp()
    {
        if (state == "Dead" || state == "Unconscious") return;
        if (weapon.GetType() == typeof(Fists))
        {
            var nearestWeapon = GetNearestWeapon();
            if (nearestWeapon) { PickUp(nearestWeapon); }
        }
        else
        {
            if (GetNearestWeapon() == null)
            {
                Throw();
            }
            else
            {
                Swap();
            }
        }

        GameObject GetNearestWeapon()
        {
            var nearestweapon = GetObject.GetNearest(gameObject, "Weapon");
            if (nearestweapon != null)
            {
                if (Vector2.Distance(nearestweapon.transform.position, gameObject.transform.position) < 15)
                {
                    return nearestweapon;
                }
            }
            return null;
        }

        void PickUp(GameObject nearestWeapon)
        {
            nearestWeapon.GetComponent<SpriteRenderer>().enabled = false;
            nearestWeapon.GetComponent<Rigidbody2D>().simulated = false;
            nearestWeapon.gameObject.transform.parent = weaponPoint;
            nearestWeapon.gameObject.transform.localPosition = Vector3.zero;
            nearestWeapon.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            weapon = nearestWeapon.GetComponent<Weapon>();
            audioSource.PlayOneShot(weapon.pickUpSound);
        }

        void Throw()
        {
            audioSource.PlayOneShot(weapon.attackSound);
            StopCoroutine("Melee");
            state = "Idle";
            ThrowWeapon(worldMousePosition);
        }

        void Swap()
        {
            var Nearestweapon = GetNearestWeapon();
            Throw();
            PickUp(Nearestweapon);
        }
    }

    void OnAttack()
    {
        if (state != "Idle") return;
        if (weapon.GetType().BaseType.Name == "Melee") { if (((Melee)weapon).cooldown == true) return; }

        if (weapon.GetType().BaseType.Name == "Gun")
        {
            Gun();
        }
        if (weapon.GetType().BaseType.Name == "Melee")
        {
            StartCoroutine(Melee());
        }

        void Gun()
        {
            Debug.DrawLine(weapon.gameObject.transform.position, worldMousePosition, Color.red, 5f);
            if (((Gun)weapon).ammo != 0) { ((Gun)weapon).Shoot(); }
            //cam.GetComponent<MainCamera>().Shake(40f);
        }

        IEnumerator Melee()
        {
            Melee melee = (Melee)weapon;
            state = "Attack";
            yield return new WaitForSeconds(melee.attackTime);
            melee.cooldown = true;
            state = "Idle";
            yield return new WaitForSeconds(melee.cooldownTime);
            melee.cooldown = false;
        }
    }

    //void OnStockAttack()
    //{
    //    if (state == "Dead" || state == "Unconscious") return;
    //    if (weapon.GetType().BaseType.Name == "Gun")
    //    {
    //        state = "Attack";
    //    }
    //}

    void OnFinishOff()
    {
        if (state == "Dead" || state == "Unconscious") return;
        GameObject enemy = GetObject.GetNearest(gameObject, typeof(Enemy));
        if (enemy.GetComponent<Enemy>().state == "Unconscious")
        {
            rb.position = enemy.transform.position;
        }
    }

    void OnAutomaticFire()
    {
        if (state == "Dead" || state == "Unconscious") return;
        //print("autofire");
    }
#endregion

    void OnCrosshair(InputValue position)
    {
        mousePosition = position.Get<Vector2>();
        worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1));
    }

    void CheckObstacles()
    {
        if (weapon.GetType().BaseType.Name == "Melee") return;
        if (polygonColliders[2].GetComponent<Overlaping>().colliders.Count > 0)
        {
            state = "Avoid";
        }
        else
        {
            state = "Idle";
        }
    }

    #region States

    #endregion
}
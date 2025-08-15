using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System;

public abstract class CharacterHealth : MonoBehaviour
{
    #region Values
    public float KnockdownForce;
    public float KnockbackForce;
    public float KnockdownTime;
    [ReadOnly] public bool IsDead;
    [ReadOnly] public bool IsUnconscious;
    [ReadOnly] public bool IsAlive { get { return !(IsDead || IsUnconscious); } }
    [ReadOnly] public bool IsAttackedFromFront;
    public event Action Died;
    public event Action BecameUnconscious;
    public event Action Knocked;
    public event Action StoodUp;
    #endregion

    #region References
    [HideInInspector] public Character Character;
    #endregion

    void Update()
    {
        Character.Body.Torso.GetComponent<SpriteRenderer>().sortingOrder = IsAlive ? 1 : 0;
    }

    public virtual void Die(Vector2 point)
    {
        Debug.Log(gameObject.name + " Died");
        Knockdown(point);
        IsUnconscious = false;
        IsDead = true;
        StopCoroutine(StandUp());
        Died?.Invoke();
    }

    public virtual void FallUnconscious(Vector2 point)
    {
        Debug.Log(gameObject.name + " Knocked");
        Knockdown(point);
        IsUnconscious = true;
        StartCoroutine(StandUp());
        BecameUnconscious?.Invoke();
    }

    protected virtual void Knockdown(Vector2 point)
    {
        IsAttackedFromFront = Vector3.Distance(transform.position + transform.right, point) < Vector3.Distance(transform.position - transform.right, point);
        Character.ItemManager.Throw();
        Character.Rigidbody.simulated = false;
        ChangeComponentsState(false);
        Knocked?.Invoke();
    }

    protected IEnumerator StandUp()
    {
        yield return new WaitForSeconds(KnockdownTime);
        IsUnconscious = false;
        Character.Rigidbody.simulated = true;
        ChangeComponentsState(true);
        StoodUp?.Invoke();
    }
    
    protected virtual void ChangeComponentsState(bool state)
    {

    }

    // private void RemoveComponents()
    // {
    //     // Component[] components = GetComponents<Component>();
    //     // foreach (Component component in components)
    //     // {
    //     //     if (component is Transform || component is SpriteRenderer || component is Animator)
    //     //         continue;

    //     //     Destroy(component);
    //     // }

    //     foreach (Transform child in transform)
    //     {
    //         if (!child.TryGetComponent<SpriteRenderer>(out _) && !child.TryGetComponent<Animator>(out _))
    //         {
    //             Destroy(child.gameObject);
    //             continue;
    //         }

    //         Component[] childComponents = child.GetComponents<Component>();
    //         foreach (Component component in childComponents)
    //             if (!(component is Transform) && !(component is SpriteRenderer) && !(component is Animator))
    //                 Destroy(component);
    //     }

    //     Component[] components = GetComponents<Component>();
    //     foreach (Component component in components)
    //         if (!(component is Transform) && !(component is SpriteRenderer) && !(component is Animator))
    //             Destroy(component);
    // }

    // protected void HandleImpact(Vector2 direction, Vector2 impactPoint) //заменить на Collision
    // {
    //     // Определяем направление "вперед" персонажа (transform.right)
    //     Vector2 forwardDirection = transform.right;

    //     // Вектор от позиции персонажа к точке удара
    //     Vector2 toImpactPoint = (impactPoint - (Vector2)transform.position).normalized;

    //     // Определяем, находится ли точка удара спереди или сзади
    //     float dotProduct = Vector2.Dot(forwardDirection, toImpactPoint);

    //     if (dotProduct > 0)
    //     {
    //         // Точка удара спереди - выполняем первое действие
    //         Debug.Log("Impact from the front");
    //         // Здесь может быть ваша логика для фронтального удара
    //     }
    //     else
    //     {
    //         // Точка удара сзади - выполняем второе действие
    //         Debug.Log("Impact from the back");
    //         // Здесь может быть ваша логика для удара в спину
    //     }

    //     // Поворачиваем персонажа в зависимости от направления direction
    //     // Сравниваем угол между текущим направлением и новым direction
    //     float angle = Vector2.SignedAngle(forwardDirection, direction);

    //     // Если угол больше 90 градусов, поворачиваем в противоположную сторону
    //     if (Mathf.Abs(angle) > 90f)
    //     {
    //         transform.right = -direction.normalized;
    //     }
    //     else
    //     {
    //         transform.right = direction.normalized;
    //     }
    // }

    // public void ApplyKnockback(Vector2 projectileDirection, Vector2 collisionNormal)
    // {
    //     //if (_isKnockbackActive) return;

    //     // Вычисляем направление отбрасывания
    //     Vector2 knockbackDirection = CalculateKnockbackDirection(projectileDirection, collisionNormal);

    //     // Применяем силу
    //     Character.Rigidbody.linearVelocity = Vector2.zero; // Сбрасываем текущую скорость
    //     Debug.Log(knockbackDirection * KnockbackForce);
    //     Character.Rigidbody.AddForce(knockbackDirection * KnockbackForce, ForceMode2D.Impulse);

    //     // Запускаем таймер
    //     //_isKnockbackActive = true;
    //     //_knockbackTimer = knockbackDuration;
    // }

    // // Вычисляем направление отбрасывания на основе направления предмета и нормали
    // private Vector2 CalculateKnockbackDirection(Vector2 projectileDirection, Vector2 collisionNormal)
    // {
    //     // Отражаем направление полёта предмета от нормали поверхности
    //     Vector2 reflectedDirection = Vector2.Reflect(projectileDirection.normalized, collisionNormal.normalized);

    //     // Добавляем немного "вверх" для визуального эффекта
    //     //reflectedDirection += Vector2.up * 0.3f;

    //     // Нормализуем итоговый вектор
    //     return reflectedDirection.normalized;
    // }

    // private void RotateInstantlyTowards(Vector2 targetPoint)
    // {
    //     Vector2 direction = targetPoint - (Vector2)transform.position;
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     transform.rotation = Quaternion.Euler(0, 0, angle);
    // }
}

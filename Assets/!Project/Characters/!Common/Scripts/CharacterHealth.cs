using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterHealth : MonoBehaviour
{
    #region Values
    [ReadOnly] public bool Dead;
    [ReadOnly] public bool Unconscious;
    #endregion

    #region References

    #endregion

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.otherCollider == BodyCollider)
    //     {
    //         //Debug.Log("BodyCollider");
    //         //if (collision.collider.GetComponent<Weapon>()) FallUnconscious();
    //         if (collision.collider.GetComponent<Bullet>()) Die();
    //     }
    // }

    // public void Die()
    // {
    //     Dead = true;
    //     ItemManager.Throw();
    //     //Throw(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    // }

    // public void FallUnconscious()
    // {
    //     Unconscious = true;
    //     ItemManager.Throw();
    //     Rigidbody.simulated = false;
    //     gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
    //     //Throw(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    // }
}

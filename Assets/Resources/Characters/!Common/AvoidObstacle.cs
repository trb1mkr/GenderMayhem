using System.Collections.Generic;
using UnityEngine;

public class AvoidObstacle : MonoBehaviour
{
    [HideInInspector] public List<Collider2D> Colliders = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Colliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Colliders.Remove(collision);
    }

    // void CheckObstacles()
    // {
    //     if (Weapon.GetType().BaseType.Name == "Melee") return;
    //     if (PolygonColliders[2].GetComponent<Overlaping>().Colliders.Count > 0)
    //     {
    //         State = "Avoid";
    //     }
    //     else
    //     {
    //         State = "Idle";
    //     }
    // }
}

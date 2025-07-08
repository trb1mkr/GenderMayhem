using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerTagFilter : MonoBehaviour
{
    #region Values
    [ReadOnly] public List<Collider2D> Colliders = new List<Collider2D>();
    #endregion

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Obstacle") && !collider.isTrigger)
            Colliders.Add(collider);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (Colliders.Contains(collider))
            Colliders.Remove(collider);
    }
}

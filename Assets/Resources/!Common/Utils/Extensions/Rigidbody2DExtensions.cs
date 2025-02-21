using System.Collections.Generic;
using UnityEngine;

public static class Rigidbody2DExtensions
{
    public static void IgnoreCollisions(this Rigidbody2D rb1, Rigidbody2D rb2, bool ignore)
    {
        if (rb1 == null || rb2 == null)
        {
            Debug.LogWarning("Один из Rigidbody2D равен null. Игнорирование коллизий не выполнено.");
            return;
        }

        List<Collider2D> colliders1 = new();
        List<Collider2D> colliders2 = new();
        rb1.GetAttachedColliders(colliders1);
        rb2.GetAttachedColliders(colliders2);

        foreach (Collider2D collider1 in colliders1)
            foreach (Collider2D collider2 in colliders2)
                Physics2D.IgnoreCollision(collider1, collider2, ignore);
    }
}

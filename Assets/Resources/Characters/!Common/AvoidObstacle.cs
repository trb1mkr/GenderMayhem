using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class AvoidObstacle : MonoBehaviour
{
    [HideInInspector] public Character Character;

    [ReadOnly] public List<Collider2D> Colliders = new List<Collider2D>();

    void Start()
    {
        Character = GetComponentInParent<Character>();
    }

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

    private void Update()
    {
        //if (Character.ItemManager.Weapon is Melee) return;
        if (Colliders.Count > 0)
            Character.StateId = CharacterStateId.Avoid;
        else if (Character.StateId == CharacterStateId.Avoid)
            Character.StateId = CharacterStateId.Idle;
    }
}

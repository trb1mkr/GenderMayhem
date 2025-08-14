using UnityEngine;

public class Enemy : Character
{
    [HideInInspector] public AIBehaviour AI;

    new private void Awake()
    {
        base.Awake();

        AI = GetComponent<AIBehaviour>();

        AI.Agent = this;
    }
}

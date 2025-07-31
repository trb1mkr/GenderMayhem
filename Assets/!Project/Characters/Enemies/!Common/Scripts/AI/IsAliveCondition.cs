using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsAlive", story: "[Agent] is not alive", category: "Conditions", id: "c009279b68482d04b8f0f4ee2e3d1749")]
public partial class IsAliveCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Enemy> Agent;

    public override bool IsTrue()
    {
        return !Agent.Value.Health.IsAlive;
    }

    // public override void OnStart()
    // {
    // }

    // public override void OnEnd()
    // {
    // }
}

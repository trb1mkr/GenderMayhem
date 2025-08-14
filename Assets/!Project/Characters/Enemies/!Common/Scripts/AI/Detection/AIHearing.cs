using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Hear", story: "[Agent] tries to hear a sound from [position]", category: "Action/Detection", id: "852c0d8fbeb58b631ddfa763b0d4a8a1")]
public partial class AIHearing : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Agent;
    [SerializeReference] public BlackboardVariable<Vector3> Position;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}


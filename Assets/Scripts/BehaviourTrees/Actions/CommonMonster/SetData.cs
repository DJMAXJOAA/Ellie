using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class SetData : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<float> playerDistance;
    public NodeProperty<Vector3> playerPosition;
    public NodeProperty<GameObject> detectChaseAI;

    protected override void OnStart()
    {
        playerDistance.Value = Vector3.SqrMagnitude(player.Value.transform.position - context.transform.position);
        playerPosition.Value = player.Value.transform.position;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
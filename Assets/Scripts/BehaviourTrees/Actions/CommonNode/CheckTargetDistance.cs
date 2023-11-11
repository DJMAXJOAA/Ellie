using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckTargetDistance : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<Transform> targetTransform;
    public NodeProperty<float> checkDistanceValue;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        // Vector3으로 비교
        if (targetTransform.Value == null)
        {
            if (Vector3.Distance(context.transform.position, targetPosition.Value) <= checkDistanceValue.Value)
            {
                return State.Success;
            } 
        }
        // Transform으로 비교
        else
        {
            if (Vector3.Distance(context.transform.position, targetTransform.Value.position) <= checkDistanceValue.Value)
            {
                return State.Success;
            }
        }
        return State.Failure;
    }
}

using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class CheckCurrentAnimationTime : ActionNode
{
    public NodeProperty<int> layerIndex;
    public NodeProperty<float> checkTimeValue;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (checkTimeValue.Value < 0.0f || checkTimeValue.Value > 1.0f)
        {
            Debug.Log("애니메이션 비율 체크 값이 잘못되었습니다.");
            return State.Failure;
        }

        var stateInfo = context.animator.GetCurrentAnimatorStateInfo(layerIndex.Value);

        var currentAnimationPer = stateInfo.normalizedTime % 1.0f;
        if (currentAnimationPer >= checkTimeValue.Value)
        {
            return State.Success;
        }

        return State.Running;
    }
}
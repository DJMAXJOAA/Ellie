using System;
using TheKiwiCoder;

[Serializable]
public class SetAnimatorRootMotion : ActionNode
{
    public NodeProperty<bool> checkRootMotion;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.animator.applyRootMotion = checkRootMotion.Value;
        return State.Success;
    }
}
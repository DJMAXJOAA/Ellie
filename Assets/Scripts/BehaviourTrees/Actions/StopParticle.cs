using System;
using TheKiwiCoder;

[Serializable]
public class StopParticle : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.particleController.StopParticle();
        return State.Success;
    }
}
using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class CheckInit : ActionNode
{
    public NodeProperty<bool> checkInit;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (checkInit.Value == false)
        {
            Debug.Log("Init");
            checkInit.Value = true;
            return State.Failure;
        }

        return State.Failure;
    }
}
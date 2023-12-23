using Managers.RangeIndicator;
using Managers.RangeIndicator.Range;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class AddRectangleRangeToTarget : ActionNode
{
    public NodeProperty<BaseRange> targetRange;
    public NodeProperty<bool> isShowRange;

    public NodeProperty<Transform> origin;
    public NodeProperty<bool> isFollowOrigin;

    public NodeProperty<float> remainTime;

    public NodeProperty<float> height;
    public NodeProperty<float> width;

    protected override void OnStart()
    {
        if (origin.Value == null)
            origin.Value = context.transform;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        targetRange.Value = RangeManager.Instance.CreateRange(new RangePayload
        {
            Type = RangeType.Rectangle,
            IsShowRange = isShowRange.Value,
            Original = origin.Value,
            IsFollowOrigin = isFollowOrigin.Value,
            RemainTime = remainTime.Value,
            Height = height.Value,
            Width = width.Value,
        }).GetComponent<BaseRange>();
        return State.Success;
    }
}

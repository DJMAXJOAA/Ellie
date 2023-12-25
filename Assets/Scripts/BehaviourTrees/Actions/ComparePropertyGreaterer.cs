using System;
using TheKiwiCoder;

[Serializable]
public class ComparePropertyGreaterer : ActionNode
{
    public NodeProperty nodeValue;
    public NodeProperty<float> compareValue;
    public NodeProperty<bool> isEqual;
    private float compare;

    private float val;

    protected override void OnStart()
    {
        compare = compareValue.Value;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var valFloat = nodeValue.reference as BlackboardKey<float>;

        if (valFloat != null)
        {
            val = valFloat.Value;
        }
        else
        {
            // BlackboardKey<float> 형변환이 실패 시 int값 사용
            var valInt = nodeValue.reference as BlackboardKey<int>;
            if (valInt != null)
            {
                // 성공적으로 형변환 된 경우, float로 캐스팅하여 값을 사용합니다.
                val = valInt.Value;
            }
            else
            {
                // 둘 다 형변환이 실패한 경우, Failure를 반환
                return State.Failure;
            }
        }

        if ((isEqual.Value && val >= compare) ||
            (isEqual.Value == false && val > compare))
        {
            return State.Success;
        }

        return State.Failure;
    }
}
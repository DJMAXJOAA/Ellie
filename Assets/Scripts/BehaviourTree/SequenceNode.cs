using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AND ���� -> �ڽ� ��带 ���������� �����Ѵ�
// �ڽ� ����߿� �ϳ��� �����ϸ� �� �ٽ���
public class SequenceNode : CompositeNode
{
    int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // �ڽ� ��尡 ������ ��ȯ�ϸ�, �� ���� ���� ������ �Ѿ
        var child = children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                current++;
                break;
        }

        // ��� �ڽ� ��尡 ������ ��ȯ�ߴٸ� ���� ��ȯ (���� ����)
        // �ƴ϶�� ���� �ڽĳ�尡 ������ ��ȯ��ų ������ ���ӵ�
        return current == children.Count ? State.Success : State.Running;
    }
}

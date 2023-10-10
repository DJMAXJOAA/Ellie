using UnityEngine;

// (�׽�Ʈ��) ������ �ൿƮ�� ���ϵ��� ������ ��� ������Ʈ
public class BehaviorTreeRunner : MonoBehaviour
{
    private BehaviorTree tree;

    private void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviorTree>();

        var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
        log1.message = "ASDFASDFSDAF~~ 111";
        var pause1 = ScriptableObject.CreateInstance<WaitNode>();

        var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.message = "ASDFASDFSDAF~~ 222";
        var pause2 = ScriptableObject.CreateInstance<WaitNode>();

        var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.message = "ASDFASDFSDAF~~ 333";
        var pause3 = ScriptableObject.CreateInstance<WaitNode>();

        var sequence = ScriptableObject.CreateInstance<SequenceNode>();
        sequence.children.Add(log1);
        sequence.children.Add(pause1);
        sequence.children.Add(log2);
        sequence.children.Add(pause2);
        sequence.children.Add(log3);
        sequence.children.Add(pause3);

        var loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.child = sequence;

        // ���� ���� ��带 �ʱ�ȭ ��Ű��
        tree.rootNode = loop;
    }

    private void Update()
    {
        tree.Update();
    }
}

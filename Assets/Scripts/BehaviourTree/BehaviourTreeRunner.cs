using UnityEngine;

// (�׽�Ʈ��) ������ �ൿƮ�� ���ϵ��� ������ ��� ������Ʈ
public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    private void Start()
    {
        tree = tree.Clone();
        tree.Bind();
    }

    private void Update()
    {
        tree.Update();
    }
}

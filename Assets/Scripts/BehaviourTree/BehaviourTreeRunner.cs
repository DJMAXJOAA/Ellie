using UnityEngine;

// (�׽�Ʈ��) ������ �ൿƮ�� ���ϵ��� ������ ��� ������Ʈ
public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    private void Start()
    {
        tree = tree.Clone();
    }

    private void Update()
    {
        tree.Update();
    }
}

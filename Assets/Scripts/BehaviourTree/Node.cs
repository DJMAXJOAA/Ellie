using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    public State state = State.Running;     // ���� ����
    public bool isStarted = false;            // ���� �� ���� �ִ���? (�ʱ�ȭ �Լ�)

    // ������ ���� �� �ϳ��� ��ȯ�Ѵ�
    public State Update()
    {
        // ����� ���� ���� ��, OnStart(); �޼���� �ʱ�ȭ
        if(!isStarted)
        {
            OnStart();
            isStarted = true;
        }

        state = OnUpdate();

        // ��带 �������� ��, ����� ���¸� �������� OnStop();
        if(state == State.Failure || state == State.Success)
        {
            OnStop();
            isStarted = false;
        }

        return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}

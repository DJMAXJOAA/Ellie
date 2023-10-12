using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    [HideInInspector] public State state = State.Running;     // ���� ����
    [HideInInspector] public bool isStarted = false;          // ���� �� ���� �ִ���? (�ʱ�ȭ �Լ�)
    [HideInInspector] public Vector2 position;                // GUI���� ����� ��ġ�� ����
    [HideInInspector] public string guid;                     // ���� Ű

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

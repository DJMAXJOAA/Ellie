using TheKiwiCoder;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public BehaviourTreeInstance behaviourTreeInstance;
    public EnemyData enemyData;

    private BlackboardKey<int> monsterHP;
    private BlackboardKey<float> monsterMovement;
    private BlackboardKey<float> monsterAttackRange;

    [SerializeField] private int hp;
    [SerializeField] private float movement;
    [SerializeField] private float attackRange;

    private void Start()
    {
        // �� ���
        behaviourTreeInstance.SetBlackboardValue<int>("monsterHP", enemyData.monsterHP);
        behaviourTreeInstance.SetBlackboardValue<float>("monsterMovement", enemyData.monsterMovement);
        behaviourTreeInstance.SetBlackboardValue<float>("monsterAttackRange", enemyData.monsterAttackRange);

        // �� ��������
        hp = behaviourTreeInstance.GetBlackboardValue<int>("monsterHP");
        movement = behaviourTreeInstance.GetBlackboardValue<float>("monsterHP");
        attackRange = behaviourTreeInstance.GetBlackboardValue<float>("monsterHP");

        // ������ ����
        monsterHP = behaviourTreeInstance.FindBlackboardKey<int>("monsterHP");
        monsterMovement = behaviourTreeInstance.FindBlackboardKey<float>("monsterMovement");
        monsterAttackRange = behaviourTreeInstance.FindBlackboardKey<float>("monsterAttackRange");
    }
    
    private void Update()
    {
        // ������ �̿��ؼ� MonoBehaviour���� �� �������ų�, ���� ����
        monsterMovement.value = monsterAttackRange.value;
        monsterAttackRange.value += 0.3f;
    }
}

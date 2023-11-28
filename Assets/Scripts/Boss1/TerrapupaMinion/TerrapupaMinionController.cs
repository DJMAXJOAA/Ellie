using Assets.Scripts.Boss1.TerrapupaMinion;
using Assets.Scripts.Managers;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using TheKiwiCoder;
using Unity.Plastic.Antlr3.Runtime.Tree;
using UnityEngine;

public class TerrapupaMinionController : BehaviourTreeController
{
    [HideInInspector] public TerrapupaMinionRootData minionData;
    [SerializeField] private TerrapupaMinionHealthBar healthBar;
    [SerializeField] private TerrapupaMinionWeakPoint[] weakPoints;

    private TicketMachine ticketMachine;

    public TicketMachine TicketMachine
    {
        get { return ticketMachine; }
    }

    public TerrapupaMinionHealthBar HealthBar
    {
        get { return healthBar; }
    }

    protected override void Awake()
    {
        base.Awake();
        minionData = rootTreeData as TerrapupaMinionRootData;
        healthBar = gameObject.GetOrAddComponent<TerrapupaMinionHealthBar>();
        weakPoints = GetComponentsInChildren<TerrapupaMinionWeakPoint>();

        InitTicketMachine();
        SubscribeEvent();
    }

    private void Start()
    {
        InitStatus();
    }

    private void SubscribeEvent()
    {
        foreach (var weakPoint in weakPoints) 
        { 
            weakPoint.SubscribeCollisionAction(OnCollidedCoreByPlayerStone);
        }
    }

    private void InitTicketMachine()
    {
        ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
        ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Terrapupa, ChannelType.BossInteraction);
    }

    private void InitStatus()
    {
        healthBar.InitData(minionData);
    }

    private void OnCollidedCoreByPlayerStone(IBaseEventPayload payload)
    {
        Debug.Log($"ReceiveDamage :: {payload}");

        CombatPayload combatPayload = payload as CombatPayload;
        PoolManager.Instance.Push(combatPayload.Attacker.GetComponent<Poolable>());
        int damage = combatPayload.Damage;

        GetDamaged(damage);
        Debug.Log($"{damage} 데미지 입음 : {minionData.currentHP.Value}");
    }

    public void GetDamaged(int damageValue)
    {
        minionData.currentHP.Value -= damageValue;
        healthBar.RenewHealthBar(minionData.currentHP.value);
        minionData.isHit.Value = true;
    }
}

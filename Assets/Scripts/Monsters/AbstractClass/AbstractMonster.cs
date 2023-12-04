using System.Collections.Generic;
using Assets.Scripts.Combat;
using Assets.Scripts.Managers;
using Assets.Scripts.Monster;
using Assets.Scripts.Monsters.Attacks;
using Assets.Scripts.Monsters.EffectStatus;
using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.UI.Monster;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;
using static Assets.Scripts.Monsters.Utility.Enums;

namespace Assets.Scripts.Monsters.AbstractClass
{
    public enum MonsterNumber
    {
        NormalSkeleton = 1000,
        AdventureSkeleton=1001,
        WizardSkeleton=1002,
        CaveBat=1003,
        GuildguardSkeleton = 1004,
    }
    public abstract class AbstractMonster : MonoBehaviour, ICombatant, IMonster
    {
        const float billboardScale = 0.003f;

        [SerializeField] public SkeletonMonsterData monsterData;
        private TicketMachine ticketMachine;
        public MonsterAttackData[] attackData = new MonsterAttackData[(int)AttackSkill.End];

        public BlackboardKey<bool> isDamaged;
        public BlackboardKey<bool> isDead;
        public BlackboardKey<bool> isReturning;

        protected bool isAttacking;
        protected AbstractAttack[] skills;
        protected Animator animator;
        public BehaviourTreeInstance behaviourTreeInstance;
        protected NavMeshAgent agent;
        private bool isHeadShot;

        public Dictionary<string, AbstractAttack> Attacks=new();

        protected MonsterEffectStatusController statusController;

        protected UIMonsterBillboard billboard;
        protected Transform billboardObject;
        public bool isBillboardOn;

        protected readonly MonsterDataContainer dataContainer = new();

        protected float currentHP;

        protected Vector3 spawnPosition;

        protected MonsterAudioController audioController;
        protected GameObject player;
        private Transform playerObj;

        private void Update()
        {
            MonsterOnPlayerForward();
        }

        public void SetPlayer(GameObject ply)
        {
            player = ply;
        }

        public AbstractAttack AddSkills(string skillName, Enums.AttackSkill attackSkill)
        {
            AbstractAttack attack = null;
            GameObject newSkill = new GameObject(skillName);
            newSkill.transform.SetParent(transform);
            newSkill.tag = gameObject.tag;
            newSkill.transform.localPosition = Vector3.zero;
            newSkill.transform.localRotation = Quaternion.Euler(Vector3.zero);
            newSkill.transform.localScale = Vector3.one;
            switch (attackSkill)
            {
                case Enums.AttackSkill.ProjectileAttack:
                    attack = newSkill.AddComponent<ProjectileAttack>();
                    break;
                case Enums.AttackSkill.BoxCollider:
                    attack = newSkill.AddComponent<BoxColliderAttack>();
                    break;
                case Enums.AttackSkill.SphereCollider:
                    attack = newSkill.AddComponent<SphereColliderAttack>();
                    break;
                case Enums.AttackSkill.WeaponAttack:
                    attack = newSkill.AddComponent<WeaponAttack>();
                    break;
                case Enums.AttackSkill.AOEAttack:
                    attack = newSkill.AddComponent<AOEPrefabAttack>();
                    break;
                case Enums.AttackSkill.FanshapeAttack:
                    attack = newSkill.AddComponent<FanShapeAttack>();
                    break;
            }
            if (attack != null)
            {
                Attacks.Add(skillName, attack);
            }

            return attack;
        }

        public void SetTicketMachine()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat, ChannelType.Monster);
        }
        public void Attack(IBaseEventPayload payload)
        {
            Debug.Log("SEND");
            CombatPayload a = payload as CombatPayload;
            Debug.Log(a.Defender.name);
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            CombatPayload combatPayload = payload as CombatPayload;
            UpdateHP(combatPayload.Damage);
        }

        public void RecieveHeadShot()
        {
            isHeadShot = true;
        }

        public void UpdateHP(float damage)
        {
            if (isReturning.value) return;
            if (currentHP == monsterData.maxHP) ShowBillboard();
            if (isHeadShot) damage *= monsterData.weakRatio;

            currentHP -= damage;
            dataContainer.CurrentHp.Value = (int)currentHP;
            isDamaged.value = true;

            if (currentHP < 1)
            {
                SendTicket();
                SetMonsterDead();
                HideBillobard();
                isDead.Value = true;
                isBillboardOn = false;
            }
            else
            {
                audioController.PlayAudio(MonsterAudioType.Hit);
            }

            isHeadShot = false;
        }

        private void SetMonsterDead()
        {
            audioController.PlayAudio(MonsterAudioType.Dead);
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            animator.Play("Dead");
            GetComponent<Collider>().enabled = false;
        }

        public void ResetMonster()
        {
            ReturnSpawnLocation();
            GetComponent<Collider>().enabled = true;
            animator.Play("IdleAttack");
            isDamaged.value = false;
            currentHP = monsterData.maxHP;
            dataContainer.CurrentHp.Value = (int)currentHP;
            isDead.value = false;
        }

        public virtual void ReturnSpawnLocation()
        { }

        public void SendTicket()
        {
            MonsterPayload monsterPayload = new();
            monsterPayload.RespawnTime = monsterData.respawnTime;
            monsterPayload.Monster = transform;
            monsterPayload.ItemDrop = monsterData.itemDropTable;
            ticketMachine.SendMessage(ChannelType.Monster, monsterPayload);
        }

        public void MonsterDead(IBaseEventPayload payload)
        {
            throw new System.NotImplementedException();
        }

        protected void InitUI()
        {
            billboardObject = Functions.FindChildByName(gameObject, "Billboard").transform;
            playerObj = player.transform.Find("PlayerObj");

            billboard = UIManager.Instance.MakeStatic<UIMonsterBillboard>(billboardObject, UIManager.UIMonsterBillboard);
            HideBillobard();
            billboard.InitBillboard(billboardObject);
        }

        public void ShowBillboard()
        {
            billboardObject.transform.localScale = Vector3.one;
            isBillboardOn = true;
        }

        public void HideBillobard()
        {
            billboardObject.transform.localScale = Vector3.zero;
        }

        public void MonsterOnPlayerForward()
        {
            if (isBillboardOn)
            {
                Vector3 direction = transform.position - player.transform.position;
                float dot = Vector3.Dot(direction.normalized, playerObj.forward.normalized);

                if(dot>0)
                {
                    ShowBillboard();
                }
                else
                {
                    HideBillobard();
                }
            }
        }

    }

}
using Data.Monster;
using UnityEngine;

namespace Monsters.AbstractClass
{
    public abstract class AbstractAttack : MonoBehaviour
    {
        [SerializeField] protected AbstractMonster monsterController;
        [SerializeField] protected MonsterAudioController audioController;
        [SerializeField] protected MonsterParticleController particleController;
        protected float attackValue;
        protected float durationTime;

        protected string owner;
        protected string prefabName;

        public float AttackableDistance { get; private set; }
        public float AttackInterval { get; private set; }

        public bool IsAttackReady { get; protected set; }

        public abstract void ActivateAttack();

        protected void InitializedBase(MonsterAttackData attackData)
        {
            attackValue = attackData.attackValue;
            durationTime = attackData.attackDuration;
            AttackInterval = attackData.attackInterval;
            AttackableDistance = attackData.attackableDistance;

            IsAttackReady = true;
            owner = transform.parent.gameObject.tag;
            audioController = transform.parent.GetComponent<MonsterAudioController>();
            particleController = transform.parent.GetComponent<MonsterParticleController>();

            monsterController = transform.parent.GetComponent<AbstractMonster>();
        }

        public virtual void InitializeBoxCollider(MonsterAttackData data)
        {
        }

        public virtual void InitializeSphereCollider(MonsterAttackData data)
        {
        }

        public virtual void InitializeProjectile(MonsterAttackData data)
        {
        }

        public virtual void InitializeWeapon(MonsterAttackData data)
        {
        }

        public virtual void InitializeAOE(MonsterAttackData data)
        {
        }

        public virtual void InitializeFanShape(MonsterAttackData data)
        {
        }

        public virtual void ReceiveDamage(IBaseEventPayload payload)
        {
        }

        public void Attack(IBaseEventPayload payload)
        {
            monsterController.Attack(payload);
        }
    }
}
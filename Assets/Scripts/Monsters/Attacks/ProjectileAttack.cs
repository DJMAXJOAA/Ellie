using System.Collections;
using Channels.Combat;
using Data.Monster;
using Managers;
using Monsters.AbstractClass;
using Monsters.Others;
using Player.StatusEffects;
using UnityEngine;

namespace Monsters.Attacks
{
    public class ProjectileAttack : AbstractAttack
    {
        [SerializeField] private GameObject projectile;
        public MonsterAttackData attackData;
        private Vector3 offset;

        public override void InitializeProjectile(MonsterAttackData data)
        {
            attackData = data;
            InitializedBase(data);
            offset = data.offset;
            projectile = ResourceManager.Instance.LoadExternResource<GameObject>(data.projectilePrefabPath);
            particleController = transform.parent.GetComponent<MonsterParticleController>();
        }

        public override void ActivateAttack()
        {
            var obj = Instantiate(projectile, transform.position + offset, transform.rotation);
            obj.GetComponent<Projectile>().SetSpeed(attackData.projectileSpeed);
            if (attackData.projectileChase == 1)
            {
                obj.GetComponent<Projectile>()
                    .ChasePlayer(transform.parent.GetComponent<AbstractMonster>().GetPlayer());
            }

            obj.GetComponent<Projectile>().spawner = gameObject.GetComponent<ProjectileAttack>();
            StartCoroutine(StartAttackReadyCount());
        }

        private IEnumerator StartAttackReadyCount()
        {
            IsAttackReady = false;
            yield return new WaitForSeconds(AttackInterval);
            IsAttackReady = true;
        }

        public void ProjectileHitPlayer(Transform otherTransform)
        {
            audioController.PlayAudio(MonsterAudioType.ProjectileHit);
            var particle = particleController.GetParticle(MonsterParticleType.ProjectileHit);
            particle.transform.position = otherTransform.position;
            particle.Play();
            SetAndAttack(attackData, otherTransform);
        }

        private void SetAndAttack(MonsterAttackData data, Transform otherTransform)
        {
            CombatPayload payload = new();
            payload.Type = data.combatType;
            payload.Attacker = transform;
            payload.Defender = otherTransform;
            payload.AttackDirection = Vector3.zero;
            payload.AttackStartPosition = transform.position;
            payload.AttackPosition = otherTransform.position;
            payload.StatusEffectName = StatusEffectName.Burn;
            payload.statusEffectduration = 3.0f;
            payload.Damage = data.attackValue;
            Attack(payload);
        }
    }
}
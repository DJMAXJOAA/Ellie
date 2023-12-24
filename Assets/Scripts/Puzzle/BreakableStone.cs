﻿using System.Collections;
using Assets.Scripts.Particle;
using Channels.Combat;
using Channels.Components;
using Combat;
using Item.Stone;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Assets.Scripts.Puzzle
{
    public class BreakableStone : SerializedMonoBehaviour, ICombatant
    {
        public GameObject destroyEffect;
        public int currentHP;
        public float shakeDuration = 0.1f;
        public float shakeMagnitude = 0.1f;

        [ShowInInspector] private bool isFrozen;
        private TicketMachine ticketMachine;

        public void Attack(IBaseEventPayload payload)
        {
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {
            var combatPayload = payload as CombatPayload;

            var attackStone = combatPayload.Attacker;

            if (attackStone.GetComponent<ExplosionStone>() != null)
            {
                DestroyStone();
            }
            else if (attackStone.GetComponent<IceStone>() != null)
            {
                SetFrozen();
            }
            else
            {
                HitByStone(combatPayload.Damage);
            }
        }

        [Button("히트 테스트", ButtonSizes.Large)]
        public void Test()
        {
            HitByStone(1);
        }

        public void InitTicketMachine(TicketMachine ticketMachine)
        {
            this.ticketMachine = ticketMachine;
        }

        private void HitByStone(int damageValue)
        {
            // 돌맹이 피격 흔들림 처리
            StartCoroutine(ShakeCoroutine());

            // 데미지 처리
            GetDamaged(damageValue);
        }

        private void GetDamaged(int damageValue)
        {
            currentHP -= damageValue;
            if (currentHP <= 0)
            {
                DestroyStone();
            }
        }

        private void SetFrozen()
        {
            // 빙결 처리
            isFrozen = true;
            Debug.Log("빙결 상태");

            var setting = GetComponent<FreezeRenderSetting>();
            setting.AddFreezeRenderer();

            // 체력 설정 -> 1 고정
            HitByStone(currentHP - 1);
        }

        private void DestroyStone()
        {
            // 파괴 파티클
            ParticleManager.Instance.GetParticle(destroyEffect, transform, 2.0f);

            // 돌 삭제
            Destroy(gameObject);
        }

        private IEnumerator ShakeCoroutine()
        {
            var elapsed = 0.0f;

            var originalPosition = transform.position;

            while (elapsed < shakeDuration)
            {
                transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
                elapsed += Time.deltaTime;
                yield return null; // 다음 프레임까지 기다림
            }

            transform.position = originalPosition; // 원래 위치로 돌아감
        }
    }
}
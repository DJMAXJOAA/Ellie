﻿using System.Collections;
using Assets.Scripts.Managers;
using Assets.Scripts.Particle;
using Channels.Portal;
using Channels.Stone;
using Channels.Type;
using Particle;
using UnityEngine;

namespace Item.Stone
{
    public class PortalStone : BaseStoneEffect
    {
        public float portalRadius = 1.0f;
        public float duration = 30.0f;
        public float cooldown = 1.0f;
        public LayerMask playerLayer;
        private bool canUsePortal = true;

        private bool isActivatePortal;
        private MeshCollider meshCollider;

        private ParticleController portalParticle;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            meshCollider = GetComponent<MeshCollider>();
            playerLayer = 1 << LayerMask.NameToLayer("Ignore Raycast");
        }

        private void Update()
        {
            if (isActivatePortal && canUsePortal)
            {
                CheckUsePortal();
            }
        }

        private void OnDisable()
        {
            // 포탈 존재 O
            if (portalParticle != null)
            {
                portalParticle.Stop();
                portalParticle = null;
            }

            rb.isKinematic = false;
            meshCollider.enabled = true;
            isActivatePortal = false;
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            if (!isActivatePortal && Type == StoneEventType.ShootStone
                                  && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isActivatePortal = true;
                ActivatePortal();
            }
        }

        private void CheckUsePortal()
        {
            var portalPosition = transform.position;
            var capsuleTop = portalPosition + Vector3.up * 2.0f;

            var hitColliders = Physics.OverlapCapsule(portalPosition, capsuleTop, portalRadius, playerLayer);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    UsePortal(hitCollider.transform);

                    break;
                }
            }
        }

        private void UsePortal(Transform player)
        {
            ticketMachine.SendMessage(ChannelType.Portal, new PortalEventPayload
            {
                Type = PortalEventType.UsePortal,
                Portal = transform,
                Player = player
            });
        }

        public void ApplyCooldown()
        {
            canUsePortal = false;
            StartCoroutine(ApplyPortalCooldown());
        }

        private IEnumerator ApplyPortalCooldown()
        {
            yield return new WaitForSeconds(cooldown);

            canUsePortal = true;
        }

        public void ActivatePortal()
        {
            // 콜라이더 제거 + 중력 제거
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            meshCollider.enabled = false;

            ticketMachine.SendMessage(ChannelType.Portal, new PortalEventPayload
            {
                Type = PortalEventType.ActivatePortal,
                Portal = transform
            });

            portalParticle = ParticleManager.Instance.GetParticle(data.skillEffectParticle, new ParticlePayload
            {
                IsLoop = true,
                Position = transform.position,
                Offset = new Vector3(0.0f, 2.0f, 0.0f)
            }).GetComponent<ParticleController>();

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "Stone_Sound_2", transform.position);

            // 지속시간 설정 + 스킬 이펙트 추가
            StartCoroutine(StartDurationCheck(duration));
        }

        private IEnumerator StartDurationCheck(float duration)
        {
            Debug.Log($"포탈 지속시간 : {duration}");

            yield return new WaitForSeconds(duration);

            Debug.Log("포탈 지속시간 종료");
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "Stone_Sound_2", transform.position);

            ticketMachine.SendMessage(ChannelType.Portal, new PortalEventPayload
            {
                Type = PortalEventType.DeactivatePortal,
                Portal = transform
            });
        }
    }
}
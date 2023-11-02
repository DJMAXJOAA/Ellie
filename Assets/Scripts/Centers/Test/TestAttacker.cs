﻿using Assets.Scripts.Combat;
using Assets.Scripts.Player;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utils;
using Channels.Combat;
using Channels.Components;
using Channels.Type;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Centers.Test
{
    public class TestAttacker : MonoBehaviour, ICombatant
    {
        public PlayerStatus playerStatus;
        public int testDamage;
        public PlayerStatusEffectName statusEffect;
        private TicketMachine ticketMachine;

        private void Awake()
        {
            SetTicketMachine();
        }
        private void SetTicketMachine()
        {
            Debug.Log("TestAttacker SetTicketMachine()");
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Combat);

            //ticketMachine.AddTicket(ChannelType.UI, new Ticket<IBaseEventPayload>());
        }
        public void Attack(IBaseEventPayload payload)
        {
            ticketMachine.SendMessage(ChannelType.Combat, payload);
        }

        public void ReceiveDamage(IBaseEventPayload payload)
        {

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                TestAttack();
            }
        }
        private void TestAttack()
        {
            CombatPayload payload = new()
            {
                Type = CombatType.Melee,
                Attacker = transform,
                Defender = playerStatus.transform,
                AttackDirection = Vector3.zero,
                AttackStartPosition = transform.position,
                AttackPosition = playerStatus.transform.position,
                PlayerStatusEffectName = statusEffect,
                Damage = testDamage
            };
            Attack(payload);
        }
    }
}
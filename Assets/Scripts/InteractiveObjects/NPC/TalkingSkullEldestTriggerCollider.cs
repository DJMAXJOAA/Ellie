﻿using System;
using Data.GoogleSheet._6100Quest;
using Player;
using UnityEngine;

namespace InteractiveObjects.NPC
{
    public class TalkingSkullEldestTriggerCollider : MonoBehaviour
    {
        private Action<Collider> firstEncounterAction;
        private PlayerQuest player;
        private Action playerExitAction;
        private Action secondEncounterAction;

        private void Update()
        {
            if (player != null &&
                player.GetQuestStatus(6101) == QuestStatus.Unaccepted &&
                Vector3.Distance(player.transform.position, transform.position) < 7.0f)
            {
                secondEncounterAction?.Invoke();
            }
        }

        private void OnDisable()
        {
            firstEncounterAction = null;
            secondEncounterAction = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponent<PlayerQuest>();
                firstEncounterAction?.Invoke(other);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!other.CompareTag("Player"))
                {
                    return;
                }

                if (player.GetQuestStatus(6101) <= QuestStatus.Accepted)
                {
                    playerExitAction?.Invoke();
                }
            }
        }

        public void SubscribeFirstEncounterAction(Action<Collider> listener)
        {
            firstEncounterAction -= listener;
            firstEncounterAction += listener;
        }

        public void SubscribeSecondEncounterAction(Action listener)
        {
            secondEncounterAction -= listener;
            secondEncounterAction += listener;
        }

        public void SubscribePlayerExitAction(Action listener)
        {
            playerExitAction -= listener;
            playerExitAction += listener;
        }
    }
}
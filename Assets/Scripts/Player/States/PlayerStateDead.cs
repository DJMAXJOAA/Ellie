﻿using Assets.Scripts.Managers;
using Channels.Type;
using Channels.UI;
using Managers.Sound;
using UnityEngine;

namespace Player.States
{
    public class PlayerStateDead : PlayerBaseState
    {
        public PlayerStateDead(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            //Controller.gameObject.tag = "Untagged";
            Controller.GetComponent<CapsuleCollider>().enabled = false;
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_sound1", Controller.PlayerObj.position);
            Controller.Anim.SetTrigger("Dead");
            Controller.canTurn = false;
            Controller.TicketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                actionType = ActionType.OpenDeathCanvas
            });
        }

        public override void OnExitState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}
﻿using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateDead : PlayerBaseState
    {
        public PlayerStateDead(PlayerController controller) : base(controller)
        {
        }

        public override void OnEnterState()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_sound1");
            Controller.Anim.SetTrigger("Dead");
            Controller.canTurn = false;
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

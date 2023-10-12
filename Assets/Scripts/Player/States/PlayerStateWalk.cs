﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateWalk : PlayerBaseState
    {
        private float moveSpeed;
        private readonly Rigidbody rb;
        public PlayerStateWalk(PlayerController controller) : base(controller)
        {
            rb = Controller.Rb;
        }

        public override void OnEnterState()
        {
            moveSpeed = Controller.WalkSpeed;
        }

        public override void OnExitState()
        {

        }
        public override void OnUpdateState()
        {
            ControlSpeed();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Controller.ChangeState(PlayerStateName.Sprint);
            }
            if (Input.GetKeyDown(KeyCode.Space) && Controller.isGrounded && Controller.canJump)
            {
                Controller.ChangeState(PlayerStateName.Jump);
            }
            if (Controller.MoveInput.magnitude == 0)
            {
                Controller.ChangeState(PlayerStateName.Idle);
            }
        }

        public override void OnFixedUpdateState()
        {
            Controller.MovePlayer(moveSpeed);
        }


        private void ControlSpeed()
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }
}

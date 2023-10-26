﻿using Assets.Scripts.StatusEffects;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    internal class PlayerStateSprint : PlayerBaseState
    {
        private float moveSpeed;
        private float expectedMoveSpeed;
        private float startMoveSpeed;
        private float interpolateTime;
        private float duration = 0.5f;
        private readonly Rigidbody rb;

        float temp;
        public PlayerStateSprint(PlayerController controller) : base(controller)
        {
            rb = controller.Rb;
        }

        public override void OnEnterState()
        {
            Controller.canTurn = true;
            Controller.isSprinting = true;
            moveSpeed = startMoveSpeed = rb.velocity.magnitude;
            expectedMoveSpeed = Controller.SprintSpeed;
            interpolateTime = 0f;
            Controller.PlayerStatus.isRecoveringStamina = false;
        }

        public override void OnExitState()
        {
            Controller.isSprinting = false;
            Controller.PlayerStatus.isRecoveringStamina = true;

        }
        public override void OnUpdateState()
        {
            InterpolateMoveSpeed();
            ControlSpeed();
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                Controller.ChangeState(PlayerStateName.Walk);
            }
            if (Input.GetKeyDown(KeyCode.Space) && Controller.isGrounded && Controller.canJump)
            {
                Controller.ChangeState(PlayerStateName.Jump);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Controller.ChangeState(PlayerStateName.Dodge);
            }
            if (Input.GetMouseButton(0))
            {
                Controller.ChangeState(PlayerStateName.Zoom);
            }
            ConsumeStamina();
        }
        private void ConsumeStamina()
        {
            temp += Time.deltaTime;
            if (temp >= 0.1f)
            {
                Controller.PlayerStatus.ConsumeStamina(Controller.PlayerStatus.SprintStaminaConsumptionPerSec / 10);
                temp = 0;
            }
            if (Controller.PlayerStatus.Stamina <= 10.0f)
            {
                Controller.ChangeState(PlayerStateName.Exhaust);
            }
        }
        public override void OnFixedUpdateState()
        {
            Controller.MovePlayer(moveSpeed);
        }

        private void InterpolateMoveSpeed()
        {
            if (interpolateTime < duration)
            {
                interpolateTime += Time.deltaTime;
                moveSpeed = Mathf.Lerp(startMoveSpeed, expectedMoveSpeed, interpolateTime / duration);
            }
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

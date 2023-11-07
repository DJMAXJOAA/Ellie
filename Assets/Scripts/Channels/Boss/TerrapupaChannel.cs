using Assets.Scripts.Combat;
using Centers;
using Channels.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Channels.Boss
{
    public enum TerrapupaAttackType
    {
        None,
        ThrowStone,
        EarthQuake,
        Roll,
        LowAttack,
    }

    public enum TerrapupaEvent
    {
        None,
        // 돌 던지기
        GripStone,
        ThrowStone,
        EarthQuake,
    }
    public class TerrapupaPayload : IBaseEventPayload
    {
        // 이벤트 타입
        public TerrapupaEvent Type { get; set; }

        // 공격력
        public int AttackValue { get; set; } 

        // 공격 쿨타임 적용 시
        public float Cooldown { get; set; }

        // 상호작용하는 대상 (객체 -> 객체)
        public Transform Sender { get; set; }

        // 상호작용 받는 대상 (객체 <- 객체)
        public Transform Receiver { get; set; }
    }

    public class BossEventPayload : IBaseEventPayload
    {
        private int intValue;
        private float floatValue;
        private Vector3 vector3Value;
        private Transform transformValue1;
        private Transform transformValue2;
        private TerrapupaAttackType attackTypeValue;
        private Transform sender;

        public BossEventPayload()
        {
            intValue = 0;
            floatValue = 0.0f;
            vector3Value = Vector3.zero;
            transformValue1 = null;
            transformValue2 = null;
            attackTypeValue = TerrapupaAttackType.None;
        }

        public int IntValue
        {
            get { return intValue; }
            set { intValue = value; }
        }

        public float FloatValue
        {
            get { return floatValue; }
            set { floatValue = value; }
        }

        public Vector3 Vector3Value
        {
            get { return vector3Value; }
            set { vector3Value = value; }
        }

        public Transform TransformValue1
        {
            get { return transformValue1; }
            set { transformValue1 = value; }
        }

        public Transform TransformValue2
        {
            get { return transformValue2; }
            set { transformValue2 = value; }
        }

        public TerrapupaAttackType AttackTypeValue
        {
            get { return attackTypeValue; }
            set { attackTypeValue = value; }
        }
        public Transform Sender
        {
            get { return sender; }
            set { sender = value; }
        }

    }

    public class TerrapupaChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            TerrapupaPayload terrapupaPayload = payload as TerrapupaPayload;

            Publish(terrapupaPayload);
        }
    }
}
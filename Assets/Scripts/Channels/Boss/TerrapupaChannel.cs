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
        // �� ������
        GripStone,
        ThrowStone,
        EarthQuake,
    }
    public class TerrapupaPayload : IBaseEventPayload
    {
        // �̺�Ʈ Ÿ��
        public TerrapupaEvent Type { get; set; }

        // ���ݷ�
        public int AttackValue { get; set; } 

        // ���� ��Ÿ�� ���� ��
        public float Cooldown { get; set; }

        // ��ȣ�ۿ��ϴ� ��� (��ü -> ��ü)
        public Transform Sender { get; set; }

        // ��ȣ�ۿ� �޴� ��� (��ü <- ��ü)
        public Transform Receiver { get; set; }
    }

    public class BossEventPayload : IBaseEventPayload
    {
        private GameObject prefabValue;
        private int intValue;
        private bool boolValue;
        private float floatValue;
        private Vector3 vector3Value;
        private Transform transformValue1;
        private Transform transformValue2;
        private Transform transformValue3;
        private TerrapupaAttackType attackTypeValue;
        private Transform sender;

        public BossEventPayload()
        {
            prefabValue = null;
            intValue = 0;
            boolValue = false;
            floatValue = 0.0f;
            vector3Value = Vector3.zero;
            transformValue1 = null;
            transformValue2 = null;
            transformValue3 = null;
            attackTypeValue = TerrapupaAttackType.None;
        }

        public GameObject PrefabValue
        {
            get { return prefabValue; }
            set { prefabValue = value; }
        }

        public int IntValue
        {
            get { return intValue; }
            set { intValue = value; }
        }

        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
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

        public Transform TransformValue3
        {
            get { return transformValue3; }
            set { transformValue3 = value; }
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
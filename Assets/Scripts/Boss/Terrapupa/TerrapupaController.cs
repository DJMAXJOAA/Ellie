﻿using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

namespace Assets.Scripts.Boss.Terrapupa
{
    public enum TerrapupaAttackType
    {
        None,
        ThrowStone,
        EarthQuake,
        Roll,
        LowAttack,
    }

    public class TerrapupaController : MonoBehaviour
    {
        [SerializeField] private List<BehaviourTree> treeList = new List<BehaviourTree>();
        [SerializeField] private BehaviourTreeInstance behaviourTreeInstance;
        [SerializeField] private TerrapupaDataInfo data;

        [SerializeField] private Transform target;
        [SerializeField] private Transform rightHand;

        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        public Transform RightHand
        {
            get { return rightHand; }
            set { rightHand = value; }
        }

        public BlackboardKey<Transform> targetTransform;
        public BlackboardKey<Transform> objectTransform;
        public BlackboardKey<Vector3> targetPosition;
        public BlackboardKey<int> currentHP;
        public BlackboardKey<float> moveSpeed;
        public BlackboardKey<bool> canThrowStone;
        public BlackboardKey<bool> canEarthQuake;
        public BlackboardKey<bool> canRoll;
        public BlackboardKey<bool> canLowAttack;
        public BlackboardKey<bool> isTempted;
        public BlackboardKey<bool> isIntake;
        public BlackboardKey<IBaseEventPayload> throwStonePayload;
        public BlackboardKey<IBaseEventPayload> occurEarthQuakePayload;

        private void Start()
        {
            InitStatus();
        }

        private void Update()
        {
            targetPosition.value = target.position;
        }

        private void InitStatus()
        {
            behaviourTreeInstance.SetBlackboardValue<Transform>("targetTransform", target);
            behaviourTreeInstance.SetBlackboardValue<Vector3>("targetPosition", target.position);
            behaviourTreeInstance.SetBlackboardValue<int>("currentHP", data.hp);
            behaviourTreeInstance.SetBlackboardValue<bool>("canThrowStone", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canEarthQuake", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canRoll", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("canLowAttack", true);
            behaviourTreeInstance.SetBlackboardValue<bool>("isTempted", false);
            behaviourTreeInstance.SetBlackboardValue<bool>("isIntake", false);

            behaviourTreeInstance.SetBlackboardValue<IBaseEventPayload>("throwStonePayload",
                new BossEventPayload { TransformValue1 = rightHand, TransformValue2 = target });
            behaviourTreeInstance.SetBlackboardValue<IBaseEventPayload>("occurEarthQuakePayload",
                new BossEventPayload { });

            targetTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("targetTransform");
            objectTransform = behaviourTreeInstance.FindBlackboardKey<Transform>("objectTransform");
            targetPosition = behaviourTreeInstance.FindBlackboardKey<Vector3>("targetPosition");
            currentHP = behaviourTreeInstance.FindBlackboardKey<int>("currentHP");
            canThrowStone = behaviourTreeInstance.FindBlackboardKey<bool>("canThrowStone");
            canEarthQuake = behaviourTreeInstance.FindBlackboardKey<bool>("canEarthQuake");
            canRoll = behaviourTreeInstance.FindBlackboardKey<bool>("canRoll");
            canLowAttack = behaviourTreeInstance.FindBlackboardKey<bool>("canLowAttack");
            isTempted = behaviourTreeInstance.FindBlackboardKey<bool>("isTempted");
            isIntake = behaviourTreeInstance.FindBlackboardKey<bool>("isIntake");
            
            throwStonePayload = behaviourTreeInstance.FindBlackboardKey<IBaseEventPayload>("throwStonePayload");
            occurEarthQuakePayload = behaviourTreeInstance.FindBlackboardKey<IBaseEventPayload>("occurEarthQuakePayload");
        }


    }
}
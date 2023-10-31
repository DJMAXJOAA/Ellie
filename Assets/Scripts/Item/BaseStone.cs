﻿using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class BaseStone : Poolable
    {
        private new Rigidbody rigidbody;

        public Rigidbody StoneRigidBody
        {
            get { return rigidbody; }
        }

        private void Awake()
        {
            rigidbody = gameObject.GetComponent<Rigidbody>();
        }

        public void SetPosition(Vector3 position)
        {
            rigidbody.position = position;
            rigidbody.rotation = Quaternion.identity;
        }

        public void MoveStone(Vector3 direction, float strength)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = false;
            rigidbody.freezeRotation = false;

            rigidbody.velocity = direction * strength;
        }
    }
}
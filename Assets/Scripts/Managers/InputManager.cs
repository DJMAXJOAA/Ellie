﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    public enum InputType
    {
        Key,
        Mouse,
        Escape,
    }

    public class InputManager : Singleton<InputManager>
    {
        public Action keyAction;
        public Action mouseAction;
        public Action escapeAction;
        public bool CanInput { get; set; } = true;

        private bool isMousePressed = false;

        public void Subscribe(InputType type, Action listener)
        {
            switch (type)
            {
                case InputType.Key:
                    keyAction -= listener;
                    keyAction += listener;
                    break;
                case InputType.Mouse:
                    mouseAction -= listener;
                    mouseAction += listener;
                    break;
                case InputType.Escape:
                    escapeAction -= listener;
                    escapeAction += listener;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                escapeAction?.Invoke();
            Debug.Log(CanInput);
            if (!CanInput)
                return;
            
            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.anyKey)
                keyAction?.Invoke();

            if (Input.GetMouseButton(0))
            {
                mouseAction?.Invoke();
                isMousePressed = true;
            }
            else
            {
                if (isMousePressed)
                    mouseAction?.Invoke();

                isMousePressed = false;
            }
            
        }

        public override void ClearAction()
        {
            keyAction = null;
            mouseAction = null;
            escapeAction = null;
        }
    }
}
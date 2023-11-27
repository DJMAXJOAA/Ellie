using System;
using System.Collections;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.PopupMenu;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Opening
{
    public class BlinkMenuButton : OpeningText
    {
        public new static readonly string Path = "Opening/MenuText";

        [SerializeField] private float blinkInterval = 1.0f;

        public PopupType PopupType { get; set; }

        private bool isBlink = false;
        private Action<PopupPayload> blinkMenuAction;

        public void Subscribe(Action<PopupPayload> listener)
        {
            blinkMenuAction -= listener;
            blinkMenuAction += listener;
        }

        private void OnDestroy()
        {
            blinkMenuAction = null;
        }

        protected override void Init()
        {
            base.Init();

            Color color = OriginColor();
            color.a = 0.0f;
            SetOriginColor(color);
            imageColor.Value = color;
        }

        protected override void BindEvents()
        {
            imagePanel.BindEvent(OnClickButton);
            imagePanel.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            imagePanel.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        // 클릭하면 새로운 UI Popup
        protected virtual void OnClickButton(PointerEventData data)
        {
            Debug.Log($"{name} 버튼 클릭됨");
            var payload = new PopupPayload();
            payload.popupType = PopupType;
            blinkMenuAction?.Invoke(payload);
        }

        protected virtual void OnPointerEnter(PointerEventData data)
        {
            StartCoroutine(BlinkPanelImage());
        }

        protected virtual void OnPointerExit(PointerEventData data)
        {
            isBlink = false;
        }

        private IEnumerator BlinkPanelImage()
        {
            isBlink = true;
            Color color = OriginColor();
            color.a = 0.0f;
            imageColor.Value = color;

            bool toRight = true;
            float timeAcc = 0.0f;
            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
            while (isBlink)
            {
                yield return wfef;

                float alpha = Mathf.Lerp(0.0f, 1.0f, timeAcc / blinkInterval);
                color.a = alpha;
                imageColor.Value = color;

                if (toRight)
                {
                    timeAcc += Time.deltaTime;
                }
                else
                {
                    timeAcc -= Time.deltaTime;
                }

                if (timeAcc > blinkInterval)
                    toRight = false;
                else if (timeAcc <= 0.0f)
                    toRight = true;
            }

            ResetImageColor();
        }
    }
}
using System;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.Opening;
using Assets.Scripts.UI.PopupMenu;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class PauseMenuButton : OpeningText
    {
        public new static readonly string Path = "Pause/PauseMenuButton";

        private enum HoverObject
        {
            HoverPanel,
        }

        [SerializeField] private UITransformData hoverPanelTransform;
        [SerializeField] private Sprite hoverImage;

        public PopupType PauseButtonPopupType { get; set; }

        private GameObject hoverPanel;
        private RectTransform hoverPanelRect;
        private Image hoverPanelImage;

        private Action<PopupPayload> pauseMenuButtonAction;

        public void InitPauseMenuButton(PopupType popupType, Action<PopupPayload> listener)
        {
            BindHoverPanel();
            InitHoverPanel();

            PauseButtonPopupType = popupType;

            pauseMenuButtonAction -= listener;
            pauseMenuButtonAction += listener;
        }

        private void OnDestroy()
        {
            pauseMenuButtonAction = null;
        }

        private void BindHoverPanel()
        {
            Bind<GameObject>(typeof(HoverObject));

            hoverPanel = GetGameObject((int)HoverObject.HoverPanel);
            hoverPanelRect = hoverPanel.GetComponent<RectTransform>();
            hoverPanelImage = hoverPanel.GetComponent<Image>();
        }

        private void InitHoverPanel()
        {
            hoverPanelImage.sprite = hoverImage;

            AnchorPresets.SetAnchorPreset(hoverPanelRect, AnchorPresets.MiddleCenter);
            hoverPanelRect.sizeDelta = hoverPanelTransform.actionRect.Value.GetSize();
            hoverPanelRect.localScale = hoverPanelTransform.actionScale.Value;

            var rect = GetRect();
            float diffX = rect.width / 2.0f + hoverPanelTransform.actionRect.Value.width / 2.0f;
            var curPos = transform.localPosition;
            curPos.x -= diffX;
            hoverPanelRect.localPosition = curPos;

            hoverPanel.gameObject.SetActive(false);
        }

        protected override void BindEvents()
        {
            gameObject.BindEvent(OnButtonClicked);
            gameObject.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            gameObject.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        private void OnButtonClicked(PointerEventData data)
        {
            PopupPayload payload = new PopupPayload();
            payload.popupType = PauseButtonPopupType;
            pauseMenuButtonAction?.Invoke(payload);
        }

        private void OnPointerEnter(PointerEventData data)
        {
            if (PauseButtonPopupType == PopupType.Escape)
                return;

            hoverPanel.gameObject.SetActive(true);
        }

        private void OnPointerExit(PointerEventData data)
        {
            if (PauseButtonPopupType == PopupType.Escape)
                return;

            hoverPanel.gameObject.SetActive(false);
        }
    }
}
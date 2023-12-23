using Data.UI.Opening;
using Managers;
using TMPro;
using UI.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Inventory.CategoryPanel
{
    public class CategoryToggleController : ToggleController
    {
        private static readonly string SoundClickPanel = "inven6";

        [SerializeField] private TextTypographyData enabledPanelData;
        [SerializeField] private TextTypographyData disabledPanelData;

        public string toggleTitle;
        public GroupType type;

        private ActivateButtonPanelHandler activateButtonPanelCallback;

        private Graphic checkMark;
        private Color normalColor;
        private Color pressedColor;
        private TextMeshProUGUI text;

        private void OnDestroy()
        {
            activateButtonPanelCallback = null;
        }

        public void Init(GroupType groupType)
        {
            InitToggle();
            SubscribeToggleEvent(OnValueChanged);

            checkMark = toggle.graphic;

            text = gameObject.FindChild<TextMeshProUGUI>(null, true);
            text.alignment = TextAlignmentOptions.Midline;
            text.text = toggleTitle;
            text.lineSpacing = 25.0f;

            text.fontSize = GetToggledSize(IsOn);
            text.color = GetToggledColor(IsOn);

            type = groupType;
            pressedColor = toggle.colors.pressedColor;
            normalColor = toggle.colors.normalColor;

            gameObject.BindEvent(OnDownHandler, UIEvent.Down);
            gameObject.BindEvent(OnUpHandler, UIEvent.Up);

            if (enabledPanelData.useOutline)
            {
                text.outlineColor = enabledPanelData.outlineColor;
                text.outlineWidth = enabledPanelData.outlineThickness;
            }
        }

        private void OnDownHandler(PointerEventData data)
        {
            checkMark.color = pressedColor;
            text.color = disabledPanelData.color;
        }

        private void OnUpHandler(PointerEventData data)
        {
            checkMark.color = normalColor;
            text.color = enabledPanelData.color;
        }

        public void Subscribe(ActivateButtonPanelHandler listener)
        {
            activateButtonPanelCallback -= listener;
            activateButtonPanelCallback += listener;
        }

        private void OnValueChanged(bool isOn)
        {
            if (isOn)
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundClickPanel, Vector3.zero);
            }

            text.fontSize = GetToggledSize(isOn);
            text.color = GetToggledColor(isOn);
            Interactable = !isOn;
            activateButtonPanelCallback?.Invoke(ToggleChangeInfo.Of(type, isOn));
        }

        private Color GetToggledColor(bool isOn)
        {
            return isOn ? enabledPanelData.color : disabledPanelData.color;
        }

        private int GetToggledSize(bool isOn)
        {
            return isOn ? (int)enabledPanelData.fontSize : (int)disabledPanelData.fontSize;
        }

        public void ActivateToggle(bool isOn)
        {
            toggle.isOn = isOn;
            toggle.onValueChanged?.Invoke(isOn);
        }
    }
}
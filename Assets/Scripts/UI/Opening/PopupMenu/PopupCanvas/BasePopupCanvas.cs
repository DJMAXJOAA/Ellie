using System;
using System.Collections.Generic;
using Data.UI.Opening;
using Data.UI.Transform;
using Managers;
using TMPro;
using UI.Framework.Popup;
using UI.Framework.Presets;
using UI.Inventory;
using UI.Opening.PopupMenu.ConfigCanvas.ButtonPanel;
using UI.Opening.PopupMenu.MenuButton;
using UnityEngine;
using Utils;

namespace UI.Opening.PopupMenu.PopupCanvas
{
    public enum PopupType
    {
        Load,
        Start,
        Config,
        Exit,
        Main,
        Escape
    }

    public struct PopupPayload
    {
        public PopupType popupType;
        public ConfigType configType;
        public ButtonType buttonType;

        public bool isOn;
    }

    public class BasePopupCanvas : UIPopup
    {
        public static readonly string Path = "PopupMenuCanvas";

        [SerializeField] private UITransformData popupMenuTransform;
        [SerializeField] private UITransformData popupTextPanelTransform;
        [SerializeField] private UITransformData popupButtonGridTransform;
        [SerializeField] private TextTypographyData popupTitleTypography;
        [SerializeField] private TextTypographyData popupButtonTypography;
        [SerializeField] private PopupData popupData;

        private readonly List<string> buttonTexts = new() { "예", "아니오" };
        private readonly List<ButtonType> buttonTypes = new() { ButtonType.Yes, ButtonType.No };

        private readonly TransformController popupBackgroundController = new();
        private readonly TransformController popupButtonGridController = new();
        private readonly List<BaseNormalMenuButton> popupButtons = new();
        private readonly TransformController popupTextPanelController = new();

        private GameObject popupBackground;

        private RectTransform popupBackgroundRect;
        private GameObject popupButtonGridPanel;
        private RectTransform popupButtonGridPanelRect;
        private PopupCanvasImpl.PopupCanvas popupCanvas;

        // 팝업 내용
        private TextMeshProUGUI popupText;
        private GameObject popupTextPanel;
        private RectTransform popupTextPanelRect;

        private PopupType popupType;

        private void LateUpdate()
        {
#if UNITY_EDITOR
            popupBackgroundController.CheckQueue(popupBackgroundRect);
            popupTextPanelController.CheckQueue(popupTextPanelRect);
            popupButtonGridController.CheckQueue(popupButtonGridPanelRect);
#endif
        }

        private void OnEnable()
        {
            popupButtons.ForEach(button => button.gameObject.SetActive(true));
        }

        private void OnDisable()
        {
            popupButtons.ForEach(button => button.gameObject.SetActive(false));
        }

        public void Subscribe(Action<PopupPayload> listener)
        {
            popupCanvas.Subscribe(listener);
        }

        public void InitPopupCanvas(PopupType type)
        {
            base.Init();

            Init();

            popupType = type;
            SetTitle(popupData.GetTitle(type));
            popupCanvas = gameObject.AddPopupCanvas(type);
        }

        protected override void Init()
        {
            Bind();
            InitGameObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            popupBackground = GetGameObject((int)GameObjects.PopupBackground);
            popupTextPanel = GetGameObject((int)GameObjects.PopupTextPanel);
            popupButtonGridPanel = GetGameObject((int)GameObjects.PopupButtonGridPanel);

            popupBackgroundRect = popupBackground.GetComponent<RectTransform>();
            popupTextPanelRect = popupTextPanel.GetComponent<RectTransform>();
            popupButtonGridPanelRect = popupButtonGridPanel.GetComponent<RectTransform>();

            popupText = popupTextPanel.FindChild<TextMeshProUGUI>(null, true);
        }

        private void InitGameObjects()
        {
#if UNITY_EDITOR
            popupMenuTransform.actionRect.Subscribe(popupBackgroundController.OnRectChange);
            popupMenuTransform.actionScale.Subscribe(popupBackgroundController.OnScaleChange);

            popupTextPanelTransform.actionRect.Subscribe(popupTextPanelController.OnRectChange);
            popupTextPanelTransform.actionScale.Subscribe(popupTextPanelController.OnScaleChange);

            popupButtonGridTransform.actionRect.Subscribe(popupButtonGridController.OnRectChange);
            popupButtonGridTransform.actionScale.Subscribe(popupButtonGridController.OnScaleChange);
#endif
            AnchorPresets.SetAnchorPreset(popupBackgroundRect, AnchorPresets.MiddleCenter);
            popupBackgroundRect.sizeDelta = popupMenuTransform.actionRect.Value.GetSize();
            popupBackgroundRect.localPosition = popupMenuTransform.actionRect.Value.ToCanvasPos();
            popupBackgroundRect.localScale = popupMenuTransform.actionScale.Value;

            AnchorPresets.SetAnchorPreset(popupTextPanelRect, AnchorPresets.MiddleCenter);
            popupTextPanelRect.sizeDelta = popupTextPanelTransform.actionRect.Value.GetSize();
            popupTextPanelRect.localPosition = popupTextPanelTransform.actionRect.Value.ToCanvasPos();
            popupTextPanelRect.localScale = popupTextPanelTransform.actionScale.Value;

            var textRect = popupText.GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(textRect, AnchorPresets.StretchAll);

            AnchorPresets.SetAnchorPreset(popupButtonGridPanelRect, AnchorPresets.MiddleCenter);
            popupButtonGridPanelRect.sizeDelta = popupButtonGridTransform.actionRect.Value.GetSize();
            popupButtonGridPanelRect.localPosition = popupButtonGridTransform.actionRect.Value.ToCanvasPos();
            popupButtonGridPanelRect.localScale = popupButtonGridTransform.actionScale.Value;

            InitButtons();
            SetPopupTitleTypography();
        }

        private void InitButtons()
        {
            // yes, no
            for (var i = 0; i < buttonTexts.Count; i++)
            {
                var button =
                    UIManager.Instance.MakeSubItem<BaseNormalMenuButton>(popupButtonGridPanelRect,
                        BaseNormalMenuButton.Path);
                button.name += $"#{buttonTexts[i]}";
                button.InitText();
                popupButtonTypography.title = buttonTexts[i];
                button.InitTypography(popupButtonTypography);
                button.InitMenuButton(buttonTypes[i]);
                button.Subscribe(OnButtonClicked);

                button.gameObject.SetActive(false);

                popupButtons.Add(button);
            }
        }

        private void SetPopupTitleTypography()
        {
            popupText.font = popupTitleTypography.fontAsset;
            popupText.fontSize = popupTitleTypography.fontSize;
            popupText.alignment = popupTitleTypography.alignmentOptions;
            popupText.lineSpacing = popupTitleTypography.lineSpacing;
        }

        private void SetTitle(string title)
        {
            popupText.text = title;
        }

        #region PopupEvent

        private void OnButtonClicked(PopupPayload payload)
        {
            payload.popupType = popupType;
            popupCanvas.Invoke(payload);
        }

        #endregion

        private enum GameObjects
        {
            PopupBackground,
            PopupTextPanel,
            PopupButtonGridPanel
        }
    }
}
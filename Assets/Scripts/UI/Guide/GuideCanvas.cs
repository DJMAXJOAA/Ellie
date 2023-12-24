using System;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Data.ActionData;
using Data.UI.Transform;
using UI.Framework.Popup;
using UI.Framework.Presets;
using UI.Inventory;
using UI.Opening.PopupMenu.MenuButton;
using UI.Opening.PopupMenu.PopupCanvas;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Guide
{
    public class GuideCanvas : UIPopup
    {
        public static readonly string Path = "Guide/GuideCanvas";

        [SerializeField] private Sprite[] guideSprites;
        [SerializeField] private Sprite[] guideButtonActiveSprites;
        [SerializeField] private Sprite[] guideButtonInactiveSprites;

        [SerializeField] private UITransformData[] transformData;
        [SerializeField] private UITransformData[] buttonsTransformData;
        private readonly List<GuideButton> buttons = new();

        // left, right
        private readonly ButtonType[] buttonTypes = { ButtonType.Yes, ButtonType.No };
        private readonly List<RectTransform> panelRects = new();

        private readonly List<GameObject> panels = new();
        private readonly Data<int> spriteIndex = new();

        private CloseButton closeButton;

        private int currentIndex;
        private Image guideImage;
        private RectTransform guideImageRect;
        private TicketMachine ticketMachine;

        private void Awake()
        {
            Init();

            InputManager.Instance.Subscribe(InputType.Escape, OnEscapeAction);
        }

        private void OnEnable()
        {
            InputManager.Instance.CanInput = false;
        }

        private void OnEscapeAction()
        {
            if (gameObject.activeSelf)
            {
                InputManager.Instance.CanInput = true;
                gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();

            spriteIndex.Value = currentIndex = 0;
            spriteIndex.Subscribe(OnIndexChanged);

            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotify);

#if UNITY_EDITOR
            TicketManager.Instance.Ticket(ticketMachine);
#endif

            gameObject.SetActive(false);
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));

            var gos = Enum.GetValues(typeof(GameObjects));
            for (var i = 0; i < gos.Length; i++)
            {
                var go = GetGameObject(i);
                panelRects.Add(go.GetComponent<RectTransform>());
                panels.Add(go);
            }

            guideImage = GetImage((int)Images.GuideImage);
            guideImageRect = guideImage.GetComponent<RectTransform>();

            closeButton = panelRects[(int)GameObjects.CloseButton].gameObject.GetComponent<CloseButton>();
            closeButton.Subscribe(OnCloseButtonClicked);
        }

        private void InitObjects()
        {
            for (var i = 0; i < transformData.Length; i++)
            {
                AnchorPresets.SetAnchorPreset(panelRects[i], AnchorPresets.MiddleCenter);
                panelRects[i].sizeDelta = transformData[i].actionRect.Value.GetSize();
                panelRects[i].localPosition = transformData[i].actionRect.Value.ToCanvasPos();
                panelRects[i].localScale = transformData[i].actionScale.Value;
            }

            AnchorPresets.SetAnchorPreset(guideImageRect, AnchorPresets.StretchAll);
            guideImageRect.sizeDelta = Vector2.zero;
            guideImageRect.localPosition = Vector3.zero;

            InitButtons();
        }

        private void InitButtons()
        {
            for (var i = 0; i < buttonsTransformData.Length; i++)
            {
                var button = UIManager.Instance.MakeSubItem<GuideButton>(transform, GuideButton.Path);
                button.name += $"#{buttonTypes[i]}";
                button.InitButton();
                button.GuideButtonImage.sprite = guideButtonActiveSprites[i];
                button.Subscribe(OnButtonClicked);
                button.GuideButtonType = buttonTypes[i];

                var buttonRect = button.GetComponent<RectTransform>();
                AnchorPresets.SetAnchorPreset(buttonRect, AnchorPresets.MiddleCenter);
                buttonRect.sizeDelta = buttonsTransformData[i].actionRect.Value.GetSize();
                buttonRect.localPosition = buttonsTransformData[i].actionRect.Value.ToCanvasPos();
                buttonRect.localScale = buttonsTransformData[i].actionScale.Value;

                buttons.Add(button);
            }

            var popupPayload = new PopupPayload();
            popupPayload.buttonType = ButtonType.Yes;
            OnButtonClicked(popupPayload);
        }

        private void OnCloseButtonClicked()
        {
            OnEscapeAction();
        }

        private void OnButtonClicked(PopupPayload payload)
        {
            // ButtonType yes -> left, no -> right
            switch (payload.buttonType)
            {
                case ButtonType.Yes:
                {
                    // -1
                    currentIndex = Math.Clamp(currentIndex - 1, 0, guideSprites.Length - 1);
                }
                    break;

                case ButtonType.No:
                {
                    // +1
                    currentIndex = Math.Clamp(currentIndex + 1, 0, guideSprites.Length - 1);
                }
                    break;

                default:
                    return;
            }

            var leftIdx = (int)ButtonType.Yes;
            var rightIdx = (int)ButtonType.No;
            if (currentIndex == 0)
            {
                buttons[leftIdx].GuideButtonImage.sprite = guideButtonInactiveSprites[leftIdx];
                buttons[leftIdx].IsActivated = false;
            }
            else
            {
                buttons[leftIdx].GuideButtonImage.sprite = guideButtonActiveSprites[leftIdx];
                buttons[leftIdx].IsActivated = true;
            }

            if (currentIndex == guideSprites.Length - 1)
            {
                buttons[rightIdx].GuideButtonImage.sprite = guideButtonInactiveSprites[rightIdx];
                buttons[rightIdx].IsActivated = false;
            }
            else
            {
                buttons[rightIdx].GuideButtonImage.sprite = guideButtonActiveSprites[rightIdx];
                buttons[rightIdx].IsActivated = true;
            }

            spriteIndex.Value = currentIndex;
        }

        private void OnIndexChanged(int value)
        {
            guideImage.sprite = guideSprites[value];
        }

        private void OnNotify(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
            {
                return;
            }

            switch (uiPayload.actionType)
            {
                case ActionType.OpenGuideCanvas:
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    gameObject.SetActive(true);
                }
                    break;
            }
        }

        private enum GameObjects
        {
            GuidePanel,
            CloseButton
        }

        private enum Images
        {
            GuideImage
        }
    }
}
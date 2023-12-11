using System;
using System.Collections.Generic;
using Assets.Scripts.ActionData;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.UI.PopupMenu;
using Assets.Scripts.Utils;
using Channels.Components;
using Channels.Type;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guide
{
    public class GuideCanvas : UIPopup
    {
        public static readonly string Path = "Guide/GuideCanvas";

        private enum GameObjects
        {
            GuidePanel,
        }

        private enum Images
        {
            GuideImage,
        }

        [SerializeField] private Sprite[] guideSprites;
        [SerializeField] private UITransformData[] transformData;
        [SerializeField] private UITransformData[] buttonsTransformData;

        private readonly List<GameObject> panels = new List<GameObject>();
        private readonly List<RectTransform> panelRects = new List<RectTransform>();
        private readonly List<GuideButton> buttons = new List<GuideButton>();

        private Image guideImage;
        private RectTransform guideImageRect;

        private int currentIndex = 0;
        private readonly Data<int> spriteIndex = new Data<int>();
        private TicketMachine ticketMachine;

        private void Awake()
        {
            Init();
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

#if UNITY_EDITOR
            TicketManager.Instance.Ticket(ticketMachine);
#endif
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<Image>(typeof(Images));

            var gos = Enum.GetValues(typeof(GameObjects));
            for (int i = 0; i < gos.Length; i++)
            {
                GameObject go = GetGameObject(i);
                panelRects.Add(go.GetComponent<RectTransform>());
                panels.Add(go);
            }

            guideImage = GetImage((int)Images.GuideImage);
            guideImageRect = guideImage.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            for (int i = 0; i < transformData.Length; i++)
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

        private readonly ButtonType[] buttonTypes = new ButtonType[] { ButtonType.Yes, ButtonType.No };

        private void InitButtons()
        {
            for (int i = 0; i < buttonsTransformData.Length; i++)
            {
                var button = UIManager.Instance.MakeSubItem<GuideButton>(transform, GuideButton.Path);
                button.name += $"#{buttonTypes[i]}";
                button.InitButton();
                button.Subscribe(OnButtonClicked);
                button.GuideButtonType = buttonTypes[i];

                var buttonRect = button.GetComponent<RectTransform>();
                AnchorPresets.SetAnchorPreset(buttonRect, AnchorPresets.MiddleCenter);
                buttonRect.sizeDelta = buttonsTransformData[i].actionRect.Value.GetSize();
                buttonRect.localPosition = buttonsTransformData[i].actionRect.Value.ToCanvasPos();
                buttonRect.localScale = buttonsTransformData[i].actionScale.Value;
            }
        }

        private void OnButtonClicked(PopupPayload payload)
        {
            // ButtonType yes -> left, no -> right
            Debug.Log($"OnButtonClicked: {payload.buttonType}");
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

            spriteIndex.Value = currentIndex;
        }

        private void OnIndexChanged(int value)
        {
            Debug.Log($"{name} OnIndexChanged {value}");
            guideImage.sprite = guideSprites[value];
        }
    }
}
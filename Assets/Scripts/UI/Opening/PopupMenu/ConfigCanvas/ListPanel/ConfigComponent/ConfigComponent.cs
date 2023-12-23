using System;
using Data.UI.Opening;
using Data.UI.Transform;
using TMPro;
using UI.Framework;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Opening.PopupMenu.ConfigCanvas.ListPanel.ConfigComponent
{
    public class ConfigComponent : UIBase
    {
        public static readonly string Path = "Opening/ConfigComponent";

        [SerializeField] private UITransformData transformData;
        [SerializeField] private TextTypographyData typographyData;

        private Action<int> componentAction;

        private Image componentImage;
        private Color imageColor;

        private TextMeshProUGUI nameText;
        private GameObject optionNext;

        private GameObject optionPrev;
        private TextMeshProUGUI optionValue;

        private RectTransform rect;

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            componentAction = null;
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
            BindEvents();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));

            optionPrev = GetGameObject((int)GameObjects.OptionPrev);
            optionNext = GetGameObject((int)GameObjects.OptionNext);

            nameText = GetText((int)Texts.NameText);
            optionValue = GetText((int)Texts.OptionValue);

            componentImage = gameObject.GetComponent<Image>();

            rect = gameObject.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            rect.sizeDelta = transformData.actionRect.Value.GetSize();
            rect.localPosition = transformData.actionRect.Value.ToCanvasPos();

            var left = optionPrev.GetOrAddComponent<OptionButton>();
            left.InitOptionButton(-1);
            left.Subscribe(OnButtonClick);

            var right = optionNext.GetOrAddComponent<OptionButton>();
            right.InitOptionButton(1);
            right.Subscribe(OnButtonClick);

            imageColor = componentImage.color;

            SetTypography(nameText, typographyData);
            SetTypography(optionValue, typographyData);
        }

        private void BindEvents()
        {
            gameObject.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            gameObject.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        private void OnPointerEnter(PointerEventData data)
        {
            imageColor.a = 1.0f;
            componentImage.color = imageColor;
        }

        private void OnPointerExit(PointerEventData data)
        {
            imageColor.a = 0.0f;
            componentImage.color = imageColor;
        }

        private void SetTypography(TextMeshProUGUI tmp, TextTypographyData data)
        {
            tmp.font = data.fontAsset;
            tmp.fontSize = data.fontSize;
            tmp.color = data.color;
            tmp.alignment = data.alignmentOptions;
            tmp.lineSpacing = data.lineSpacing;
        }

        public void SetConfigData(string configName,
            bool readOnly,
            Action<int> onIndexChanged)
        {
            optionPrev.SetActive(!readOnly);
            optionNext.SetActive(!readOnly);

            nameText.text = configName;

            componentAction -= onIndexChanged;
            componentAction += onIndexChanged;
        }

        private void OnButtonClick(int value)
        {
            componentAction?.Invoke(value);
        }

        public void OnOptionValueChanged(string value)
        {
            optionValue.text = value;
        }

        private enum GameObjects
        {
            OptionPrev,
            OptionNext
        }

        private enum Texts
        {
            NameText,
            OptionValue
        }
    }
}
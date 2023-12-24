using Data.UI.Opening;
using TMPro;
using UI.Framework;
using UI.Framework.Presets;
using UnityEngine;

namespace UI.Inventory.DesctiptionPanel
{
    public class DescriptionNamePanel : UIBase
    {
        private TextMeshProUGUI descNameText;

        private RectTransform rect;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<TextMeshProUGUI>(typeof(Texts));

            descNameText = GetText((int)Texts.DescriptionNameText);
        }

        private void InitObjects()
        {
            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.DescNameRect.GetSize();
            rect.localPosition = InventoryConst.DescNameRect.ToCanvasPos();
        }

        public void SetTypographyData(TextTypographyData data)
        {
            descNameText.color = data.color;
            descNameText.fontSize = data.fontSize;
            descNameText.alignment = data.alignmentOptions;
            descNameText.lineSpacing = data.lineSpacing;

            descNameText.font = data.fontAsset;
        }

        public void SetDescriptionName(string descName)
        {
            descNameText.text = descName;
        }

        private enum Texts
        {
            DescriptionNameText
        }
    }
}
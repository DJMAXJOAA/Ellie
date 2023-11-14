using Assets.Scripts.UI.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class ImageAndTextArea : UIBase
    {
        public static readonly string GoldPath = "UI/Item/Gold2";
        public static readonly string StonePiecePath = "UI/Item/StonePiece2";

        private enum Images
        {
            Image
        }

        private enum Texts
        {
            Text
        }

        public RectTransform Rect { get; private set; }
        public Image Image { get; private set; }
        public TextMeshProUGUI Text { get; private set; }

        private readonly Color fontColor = Color.white;
        private readonly int fontSize = 20;
        private readonly float lineHeight = 25;

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
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));

            Rect = GetComponent<RectTransform>();
            Image = GetImage((int)Images.Image);
            Text = GetText((int)Texts.Text);
        }

        private void InitObjects()
        {
            Text.color = fontColor;
            Text.fontSize = fontSize;
            Text.lineSpacing = lineHeight;
            Text.alignment = TextAlignmentOptions.Bottom;
        }

        public void OnGoodsCountChanged(int value)
        {
            if (value < 0)
                value = 0;
            if (value > 9999)
                value = 9999;

            Text.text = value.ToString();
        }
    }
}
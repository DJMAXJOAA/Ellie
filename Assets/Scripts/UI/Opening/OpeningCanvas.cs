using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Framework.Static;
using Assets.Scripts.UI.Inventory;
using Data.UI.Opening;
using UnityEngine;

namespace Assets.Scripts.UI.Opening
{
    public class OpeningCanvas : UIStatic
    {
        public static readonly string Path = "Opening/OpeningCanvas";

        private enum GameObjects
        {
            TitlePanel,
            MenuPanel,
        }

        private GameObject titlePanel;
        private GameObject menuPanel;

        private RectTransform titlePanelRect;
        private RectTransform menuPanelRect;

        [SerializeField] private UITransformData titleTransformData;
        [SerializeField] private UITransformData menuTransformData;
        [SerializeField] private OpeningTextData titleTypographyData;

        private readonly TransformController titleController = new TransformController();
        private readonly TransformController menuController = new TransformController();

        private OpeningText title;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));

            titlePanel = GetGameObject((int)GameObjects.TitlePanel);
            menuPanel = GetGameObject((int)GameObjects.MenuPanel);

            titlePanelRect = titlePanel.GetComponent<RectTransform>();
            menuPanelRect = menuPanel.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
#if UNITY_EDITOR
            titleTransformData.actionRect.Subscribe(titleController.OnRectChange);
            titleTransformData.actionScale.Subscribe(titleController.OnScaleChange);

            menuTransformData.actionRect.Subscribe(menuController.OnRectChange);
            menuTransformData.actionScale.Subscribe(menuController.OnScaleChange);
#endif

            AnchorPresets.SetAnchorPreset(titlePanelRect, AnchorPresets.MiddleCenter);
            titlePanelRect.sizeDelta = titleTransformData.actionRect.Value.GetSize();
            titlePanelRect.localPosition = titleTransformData.actionRect.Value.ToCanvasPos();
            titlePanelRect.localScale = titleTransformData.actionScale.Value;

            AnchorPresets.SetAnchorPreset(menuPanelRect, AnchorPresets.MiddleCenter);
            menuPanelRect.sizeDelta = menuTransformData.actionRect.Value.GetSize();
            menuPanelRect.localPosition = menuTransformData.actionRect.Value.ToCanvasPos();
            menuPanelRect.localScale = menuTransformData.actionScale.Value;

            title = UIManager.Instance.MakeSubItem<OpeningText>(titlePanelRect, OpeningText.Path);

            title.InitOpeningText();
            title.InitTypography(titleTypographyData);
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            titleController.CheckQueue(titlePanelRect);
            menuController.CheckQueue(menuPanelRect);
#endif
        }
    }
}
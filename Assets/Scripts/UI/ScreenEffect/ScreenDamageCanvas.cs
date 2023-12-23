using System.Collections;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Managers;
using UI.Framework.Popup;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.ScreenEffect
{
    public class ScreenDamageCanvas : UIPopup
    {
        public static readonly string Path = "ScreenEffect/ScreenDamageCanvas";

        [SerializeField] private Color blurColor;
        [SerializeField] [Range(0.0f, 5.0f)] private float blurDuration = 0.15f;
        [SerializeField] private float baseClarity = 0.15f;

        private Image blurEffectImage;
        private Color blurEndColor;

        private Color blurStartColor;

        private Coroutine fadeInCoroutine;
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
        }

        private void Bind()
        {
            Bind<Image>(typeof(Images));

            blurEffectImage = GetImage((int)Images.BlurEffect);
        }

        private void InitObjects()
        {
            blurStartColor = blurEndColor = blurColor;
            blurStartColor.a = 0.0f;
            blurEndColor.a = baseClarity;

            blurEffectImage.color = blurStartColor;

            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);
            ticketMachine.RegisterObserver(ChannelType.UI, OnNotify);

#if UNITY_EDITOR
            TicketManager.Instance.Ticket(ticketMachine);
#endif
        }

        private void OnNotify(IBaseEventPayload payload)
        {
            if (payload is not UIPayload uiPayload)
            {
                return;
            }

            switch (uiPayload.actionType)
            {
                case ActionType.ShowBlurEffect:
                {
                    if (fadeInCoroutine != null)
                    {
                        StopCoroutine(fadeInCoroutine);
                    }

                    fadeInCoroutine = StartCoroutine(FadeInBlur(uiPayload.blurClarity));
                }
                    break;
            }
        }

        private IEnumerator FadeInBlur(float clarity)
        {
            var timeAcc = 0.0f;
            var wfef = new WaitForEndOfFrame();

            blurEndColor.a = clarity <= 0.0f ? baseClarity : clarity;

            while (timeAcc <= blurDuration)
            {
                blurEffectImage.color = Color.Lerp(blurStartColor, blurEndColor, timeAcc / blurDuration);

                timeAcc += Time.deltaTime;
                yield return wfef;
            }

            yield return new WaitForSeconds(blurDuration);

            blurEffectImage.color = blurStartColor;
        }

        private enum Images
        {
            BlurEffect
        }
    }
}
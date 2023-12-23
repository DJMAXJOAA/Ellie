using Channels.Components;
using Channels.Type;
using Channels.UI;
using Managers;
using UnityEngine;
using Utils;

namespace UI.ScreenEffect
{
    public class ScreenDamageCanvasTestClient : MonoBehaviour
    {
        public float clarity = 0.5f;
        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);
#if UNITY_EDITOR
            TicketManager.Instance.Ticket(ticketMachine);
#endif
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var payload = UIPayload.Notify();
                payload.actionType = ActionType.ShowBlurEffect;
                payload.blurClarity = clarity;

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }
        }
    }
}
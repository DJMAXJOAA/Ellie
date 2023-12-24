using Assets.Scripts.Managers;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using UnityEngine;
using Utils;

namespace UI.Quest
{
    public class QuestCanvasTestClient : MonoBehaviour
    {
        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);

            TicketManager.Instance.Ticket(ticketMachine);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var payload = new UIPayload();

                payload.uiType = UIType.Notify;
                payload.actionType = ActionType.ClearQuest;

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                var payload = new UIPayload();

                payload.uiType = UIType.Notify;
                payload.actionType = ActionType.SetQuestName;
                payload.questInfo = QuestInfo.Of(null, "Test", null);

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                var payload = new UIPayload();

                payload.uiType = UIType.Notify;
                payload.actionType = ActionType.SetQuestDesc;
                payload.questInfo = QuestInfo.Of(null, null, "TestDesc");

                ticketMachine.SendMessage(ChannelType.UI, payload);
            }
        }
    }
}
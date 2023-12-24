using Channels.Components;
using Channels.Type;
using Channels.UI;
using UI.Video;
using UnityEngine;
using Utils;

namespace Ending
{
    public class VideoStartClient : MonoBehaviour
    {
        public VideoCanvas canvas;

        private TicketMachine ticketMachine;

        private void Awake()
        {
            ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.UI);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                canvas.EndVideo();
            }
        }

        public void SendEndingPayload()
        {
            // 동영상 재생 페이로드
            var payload = UIPayload.Notify();
            payload.actionType = ActionType.PlayVideo;
            ticketMachine.SendMessage(ChannelType.UI, payload);
        }
    }
}
using System;
using System.Collections.Generic;
using Assets.Scripts.Data.Channels;
using Channels;
using Channels.Components;
using Channels.Type;
using Channels.Utils;
using UnityEngine;

namespace Centers
{
    public struct TicketBox
    {
        private ChannelType type;
        private Ticket<IBaseEventPayload> ticket;

        private TicketBox(ChannelType type, Ticket<IBaseEventPayload> ticket)
        {
            this.type = type;
            this.ticket = ticket;
        }

        public static TicketBox Of(ChannelType type, Ticket<IBaseEventPayload> ticket)
        {
            return new TicketBox(type, ticket);
        }

        public void Ticket(IDictionary<ChannelType, BaseEventChannel> channels)
        {
            ticket.Subscribe(channels[type].ReceiveMessage);
        }
    }

    public class BaseCenter : MonoBehaviour
    {
        [SerializeField] private BaseChannelTypeSo baseChannelTypeSo;

        private readonly IDictionary<ChannelType, BaseEventChannel> channels =
            new Dictionary<ChannelType, BaseEventChannel>();

        protected virtual void Init()
        {
            InitChannels();
        }

        private void InitChannels()
        {
            int length = baseChannelTypeSo.channelTypes.Length;
            for (int i = 0; i < length; i++)
            {
                ChannelType type = baseChannelTypeSo.channelTypes[i];
                channels[type] = ChannelUtil.MakeChannel(type);
            }
        }

        protected virtual void Start()
        {
            // !TODO Start() 메서드에서 CheckTicket 메서드를 호출하여 GameObject의 티켓을 만들어야 합니다. 
        }

        protected void CheckTicket(GameObject go)
        {
            var machines =  go.GetComponentsInChildren<TicketMachine>();
            if (machines.Length == 0)
                return;

            foreach (var machine in machines)
            {
                machine.Ticket(channels);
                machine.Subscribe(OnAddTicket);
            }
        }

        private void OnAddTicket(TicketBox box)
        {
            box.Ticket(channels);
        }

        public void AddChannel(ChannelType type, BaseEventChannel channel)
        {
            if (channels.ContainsKey(type))
            {
                Debug.LogWarning($"{type}'s channel is already exist");
            }
            else
            {
                channels[type] = channel;
            }
        }
    }
}
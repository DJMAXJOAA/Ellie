using System;
using Managers;

namespace Channels.Components
{
    public sealed class Ticket
    {
        private Action<IBaseEventPayload> channelNotifyAction;
        private Action<IBaseEventPayload> sendMessageAction;

        public static void RegisterObserver(Ticket ticket, Action<IBaseEventPayload> observer)
        {
            ticket.channelNotifyAction -= observer;
            ticket.channelNotifyAction += observer;
        }

        private void Subscribe(Action<IBaseEventPayload> listener)
        {
            sendMessageAction -= listener;
            sendMessageAction += listener;
        }

        public void Subscribe(BaseEventChannel channel)
        {
            Subscribe(channel.ReceiveMessage);
            channel.Subscribe(channelNotifyAction);
        }

        // Ticket -> Channel
        public void Publish(IBaseEventPayload payload)
        {
            sendMessageAction?.Invoke(payload);
        }


        // Channel -> Ticket
        public void Notify(IBaseEventPayload payload)
        {
            channelNotifyAction?.Invoke(payload);
        }
    }
}
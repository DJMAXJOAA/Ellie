using System;
using Managers;

namespace Channels
{
    public abstract class BaseEventChannel
    {
        private Action<IBaseEventPayload> sendMessageAction;

        public abstract void ReceiveMessage(IBaseEventPayload payload);

        protected void Publish(IBaseEventPayload payload)
        {
            sendMessageAction?.Invoke(payload);
        }

        public void Subscribe(Action<IBaseEventPayload> listener)
        {
            sendMessageAction -= listener;
            sendMessageAction += listener;
        }
    }
}
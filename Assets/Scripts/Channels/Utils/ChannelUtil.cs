using System;
using Channels.Combat;
using Channels.Type;
using Channels.UI;

namespace Channels.Utils
{
    public static class ChannelUtil
    {
        public static BaseEventChannel MakeChannel(ChannelType type)
        {
            BaseEventChannel channel = null;
            switch (type)
            {
                case ChannelType.Combat:
                    channel = new CombatChannel();
                    break;
                case ChannelType.UI:
                    channel = new UIChannel();
                    break;
            }

            return channel;
        }
    }
}
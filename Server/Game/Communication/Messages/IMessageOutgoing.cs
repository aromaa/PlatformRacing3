using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal interface IMessageOutgoing
    {
        byte[] GetBytes();
    }
}

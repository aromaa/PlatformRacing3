using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Platform_Racing_3_Server.Game.User.Identifiers
{
    public interface IUserIdentifier
    {
        bool Matches(uint userId, uint socketId, IPAddress ipAddress);
    }
}

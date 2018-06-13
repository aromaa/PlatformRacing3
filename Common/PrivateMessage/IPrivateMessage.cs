using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Platform_Racing_3_Common.PrivateMessage
{
    public interface IPrivateMessage
    {
        uint Id { get; }
        uint ReceiverId { get; }

        uint SenderId { get; }
        string SenderUsername { get; }
        Color SenderNameColor { get; }

        string Title { get; }
        string Message { get; }

        DateTime SentTime { get; }
    }
}

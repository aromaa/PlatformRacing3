using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.Collections;
using Net.Sockets;

namespace Platform_Racing_3_Server.Collections.Matcher
{
    internal sealed class ExcludeSocketMatcher : ISocketMatcher
    {
        internal ISocket Not { get; }

        internal ExcludeSocketMatcher(ISocket not)
        {
            this.Not = not;
        }

        public bool Matches(ISocket socket) => this.Not != socket;
    }
}

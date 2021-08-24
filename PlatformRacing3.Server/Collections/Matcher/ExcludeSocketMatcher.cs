using Net.Collections;
using Net.Sockets;

namespace PlatformRacing3.Server.Collections.Matcher
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

using Net.Collections;
using Net.Connections;
using Platform_Racing_3_Server.Game.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Transactions;

namespace Platform_Racing_3_Server.Collections
{
    internal class ClientSessionCollectionLimited : ClientSessionCollection
    {
        private const int FULL_CAPACITY = int.MinValue;

        private int Capacity;

        internal ClientSessionCollectionLimited(Action<ClientSession, CilentCollectionRemoveReason> callback, int capacity) : base(callback)
        {
            this.Capacity = capacity;
        }

        protected override bool OnTryAdd(SocketConnection connection, ClientSession metadata)
        {
            if (this.Contains(connection))
            {
                return false;
            }

            while (true)
            {
                int capacity = this.Capacity;
                if (capacity > 0)
                {
                    if (Interlocked.CompareExchange(ref this.Capacity, capacity - 1, capacity) == capacity)
                    {
                        if (base.OnTryAdd(connection, metadata))
                        {
                            return true;
                        }
                        else
                        {
                            this.TryAddSlotBack();

                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        protected override void OnRemoved(SocketConnection connection, CilentCollectionRemoveReason reason)
        {
            this.TryAddSlotBack();

            base.OnRemoved(connection, reason);
        }

        internal void TryAddSlotBack()
        {
            while (true)
            {
                int capacity = this.Capacity;
                if (capacity >= 0)
                {
                    if (Interlocked.CompareExchange(ref this.Capacity, capacity + 1, capacity) == capacity)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        internal bool MarkFullIfFull() => this.MarkFullIf(0);
        internal bool MarkFullIf(int condition)
        {
            while (true)
            {
                int capacity = this.Capacity;
                if (capacity == condition)
                {
                    if (Interlocked.CompareExchange(ref this.Capacity, ClientSessionCollectionLimited.FULL_CAPACITY, capacity) == capacity)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }

            return false;
        }

        internal int MarkFull()
        {
            while (true)
            {
                int capacity = this.Capacity;
                if (capacity >= 0)
                {
                    return Interlocked.Exchange(ref this.Capacity, 0);
                }
                else
                {
                    break;
                }
            }

            return 0;
        }

        internal bool IsFull => this.Capacity <= 0;
    }
}

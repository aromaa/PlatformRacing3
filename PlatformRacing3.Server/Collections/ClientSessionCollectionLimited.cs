using Net.Sockets;
using PlatformRacing3.Server.Game.Client;

namespace PlatformRacing3.Server.Collections;

internal class ClientSessionCollectionLimited : ClientSessionCollection
{
	private const int FULL_CAPACITY = int.MinValue;

	private int Capacity;

	internal ClientSessionCollectionLimited(int capacity, Action<ClientSession> removeCallback = null) : base(removeCallback: removeCallback)
	{
		this.Capacity = capacity;
	}

	internal override bool TryAdd(ClientSession session)
	{
		if (this.Contains(session)) //Fast path
		{
			return false;
		}

		while (true)
		{
			int capacity = this.Capacity;
			if (capacity <= 0)
			{
				return false;
			}

			if (Interlocked.CompareExchange(ref this.Capacity, capacity - 1, capacity) == capacity)
			{
				if (base.TryAdd(session))
				{
					return true;
				}

				this.TryAddSlotBack();

				return false;
			}
		}
	}

	protected override void OnRemoved(ISocket socket, ref ClientSession session)
	{
		this.TryAddSlotBack();

		base.OnRemoved(socket, ref session);
	}

	internal void TryAddSlotBack()
	{
		while (true)
		{
			int capacity = this.Capacity;
			if (capacity < 0)
			{
				break;
			}

			if (Interlocked.CompareExchange(ref this.Capacity, capacity + 1, capacity) == capacity)
			{
				break;
			}
		}
	}

	internal bool MarkFullIfFull() => this.MarkFullIf(0);
	internal bool MarkFullIf(int condition) => Interlocked.CompareExchange(ref this.Capacity, ClientSessionCollectionLimited.FULL_CAPACITY, condition) == condition;

	internal int MarkFull()
	{
		while (true)
		{
			int capacity = this.Capacity;
			if (capacity < 0)
			{
				break;
			}

			if (this.MarkFullIf(capacity))
			{
				return capacity;
			}
		}

		return 0;
	}

	internal bool IsFull => this.Capacity <= 0;
}
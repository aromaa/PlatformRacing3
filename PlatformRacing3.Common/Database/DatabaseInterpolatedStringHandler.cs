using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NpgsqlTypes;

namespace PlatformRacing3.Common.Database
{
	[InterpolatedStringHandler]
	public ref struct DatabaseInterpolatedStringHandler
	{
		private readonly DatabaseConnection dbConnection;

		private readonly StringBuilder stringBuilder;

		private int counter;

		public DatabaseInterpolatedStringHandler(int literalLength, int formattedCount, DatabaseConnection dbConnection)
		{
			this.dbConnection = dbConnection;

			this.stringBuilder = new(literalLength + (formattedCount * 3));

			this.counter = 1;

			dbConnection.ResetState();
		}

		public void AppendLiteral(string value)
		{
			this.stringBuilder.Append(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AppendFormatted<T>(T value)
		{
			if (typeof(T) == typeof(ushort))
			{
				this.dbConnection.AddParamWithValue(Convert.ToInt32(value));
			}
			else if (typeof(T) == typeof(uint))
			{
				this.dbConnection.AddParamWithValue(value, NpgsqlDbType.Oid);
			}
			else if (typeof(T) == typeof(ulong))
			{
				this.dbConnection.AddParamWithValue(Convert.ToInt64(value));
			}
			else if (typeof(T) == typeof(uint[]))
			{
				this.dbConnection.AddParamWithValue(value, NpgsqlDbType.Array | NpgsqlDbType.Oid);
			}
			else
			{
				this.dbConnection.AddParamWithValue(value);
			}

			this.stringBuilder.Append('$');
			this.stringBuilder.Append(this.counter++);
		}

		public string Text => this.stringBuilder.ToString();
	}
}

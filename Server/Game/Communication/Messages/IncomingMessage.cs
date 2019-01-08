using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal class IncomingMessage
    {
        private byte[] Data;
        private int Pointer;

        internal IncomingMessage(byte[] data)
        {
            this.Data = data;
            this.Pointer = 0;
        }

        internal int Remaining => this.Data.Length - this.Pointer;

        internal byte ReadByte() => this.Data[this.Pointer++];
        internal byte[] ReadBytes(int amount)
        {
            byte[] data = new byte[amount];
            for(int i = 0; i < amount; i++)
            {
                data[i] = this.ReadByte();
            }

            return data;
        }

        internal bool ReadBool() => this.ReadByte() == 1;
        
        internal int ReadInt() => this.Data[this.Pointer++] << 24 | this.Data[this.Pointer++] << 16 | this.Data[this.Pointer++] << 8 | this.Data[this.Pointer++];
        internal uint ReadUInt() => (uint)this.ReadInt();

        internal short ReadShort() => (short)(this.Data[this.Pointer++] << 8 | this.Data[this.Pointer++]);
        internal ushort ReadUShort() => (ushort)this.ReadShort();

        internal double ReadDouble()
        {
            Span<byte> reverse = stackalloc byte[8];
            for(int i = 0; i < reverse.Length; i++)
            {
                reverse[i] = this.ReadByte();
            }

            reverse.Reverse();

            return BitConverter.ToDouble(reverse);
        }

        internal float ReadFloat()
        {
            try
            {
                return BitConverter.ToSingle(this.Data, this.Pointer);
            }
            finally
            {
                this.Pointer += 4;
            }
        }

        internal string ReadString() => Encoding.UTF8.GetString(this.ReadBytes(this.ReadUShort()));
        internal string ReadStringUnsafe() => Encoding.UTF8.GetString(this.ReadBytes(this.Remaining));
    }
}

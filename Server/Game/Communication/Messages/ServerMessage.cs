using System;
using System.Collections.Generic;
using System.Text;

namespace Platform_Racing_3_Server.Game.Communication.Messages
{
    internal class ServerMessage
    {
        private List<byte> Data { get; }
        
        public ServerMessage()
        {
            //Reserve two bytes for length
            this.Data = new List<byte>()
            {
                0,
                0,
            };
        }

        public void WriteByte(byte value) => this.Data.Add(value);
        public void WriteBytes(byte[] bytes)
        {
            foreach(byte byte_ in bytes)
            {
                this.WriteByte(byte_);
            }
        }

        public void WriteBool(bool value) => this.WriteByte(value ? (byte)1 : (byte)0);

        public void WriteUShort(ushort value) => this.WriteShort((short)value);
        public void WriteShort(short value)
        {
            this.WriteByte((byte)(value >> 8));
            this.WriteByte((byte)value);
        }

        public void WriteUInt(uint value) => this.WriteInt((int)value);
        public void WriteInt(int value)
        {
            this.WriteByte((byte)(value >> 24));
            this.WriteByte((byte)(value >> 16));
            this.WriteByte((byte)(value >> 8));
            this.WriteByte((byte)value);
        }

        public void WriteDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            Array.Reverse(bytes);

            this.Data.AddRange(bytes);
        }

        public void WriteFloat(float value)
        {
            this.Data.AddRange(BitConverter.GetBytes(value));
        }

        public void WriteString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            this.WriteUShort((ushort)bytes.Length);
            this.WriteBytes(bytes);
        }

        private void WriteLength()
        {
            int count = this.Data.Count - 2; //Don't include the length bytes

            this.Data[0] = (byte)(count >> 8);
            this.Data[1] = (byte)count;
        }

        public byte[] GetBytes()
        {
            this.WriteLength();

            return this.Data.ToArray();
        }
    }
}

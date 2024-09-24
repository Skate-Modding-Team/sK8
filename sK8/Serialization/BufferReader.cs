namespace sK8.Serialization
{
    internal class BufferReader : BinaryReader
    {
        public BufferReader(Stream stream) : base(stream) { }

        public short ReadInt16BE()
        {
            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToInt16(data);
        }

        public ushort ReadUInt16BE()
        {
            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToUInt16(data);
        }

        public int ReadInt32BE()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToInt32(data);
        }

        public uint ReadUInt32BE()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToUInt32(data);
        }

        public long ReadInt64BE()
        {
            var data = base.ReadBytes(8);
            Array.Reverse(data);
            return BitConverter.ToInt64(data);
        }

        public ulong ReadUInt64BE()
        {
            var data = base.ReadBytes(8);
            Array.Reverse(data);
            return BitConverter.ToUInt64(data);
        }

        public Half ReadFloat16BE()
        {
            var data = base.ReadBytes(2);
            Array.Reverse(data);
            return BitConverter.ToHalf(data);
        }

        public float ReadFloat32BE()
        {
            var data = base.ReadBytes(4);
            Array.Reverse(data);
            return BitConverter.ToSingle(data);
        }

        public double ReadFloat64BE()
        {
            var data = base.ReadBytes(8);
            Array.Reverse(data);
            return BitConverter.ToDouble(data);
        }

        public string ReadString(int length)
        {
            var data = base.ReadBytes(length);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        public string ReadNTString()
        {
            List<byte> data = new List<byte>();
            while (base.PeekChar() != 0x00)
            {
                data.Add(base.ReadByte());
            }
            base.ReadByte();
            return System.Text.Encoding.ASCII.GetString(data.ToArray());
        }
    }
}

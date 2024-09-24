namespace sK8.Serialization
{
    internal class BufferWriter : BinaryWriter
    {
        internal BufferWriter(Stream stream) : base(stream) { }

        public void WriteBE(short val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteBE(int val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteBE(long val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteBE(ushort val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteBE(uint val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteBE(ulong val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteBE(Half val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteBE(float val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteBE(double val)
        {
            base.Write(BitConverter.GetBytes(val).Reverse().ToArray());
        }

        public void WriteString(string str)
        {
            base.Write(System.Text.Encoding.ASCII.GetBytes(str));
        }

        public void WriteNTString(string str)
        {
            WriteString(str);
            base.Write((byte)0x00);
        }

        public void WritePadding(int count)
        {
            for (int i = 0; i < count; i++)
            {
                base.Write((byte)0x00);
            }
        }
    }
}

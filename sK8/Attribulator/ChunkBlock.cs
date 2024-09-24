using sK8.Serialization;

namespace sK8.Attribulator
{
    internal abstract class ChunkBlock : ISerializable
    {
        protected uint Type;
        protected uint Size;

        protected ChunkBlock(string type, uint size)
        {

        }

        public virtual uint GetBufferSize()
        {
            return 8;
        }

        public virtual byte[] Serialize()
        {
            return Serializer.Serialize(Type, Size);
        }

        public virtual void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, Type, Size);
            Type = (uint)vals[0];
            Size = (uint)vals[1];
        }
    }
}

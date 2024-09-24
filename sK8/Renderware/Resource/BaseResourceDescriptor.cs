using sK8.Serialization;

namespace sK8.Renderware.Resource
{
    /**
    <summary>An object describing size and alignment requirements of a single memory block.</summary>
    */
    internal class BaseResourceDescriptor : ISerializable
    {
        internal uint Size;
        internal uint Alignment;

        internal BaseResourceDescriptor(uint size, uint alignment)
        {
            Size = size;
            Alignment = alignment;
        }

        public uint GetBufferSize()
        {
            return 8;
        }

        public byte[] Serialize()
        {
            return Serializer.Serialize(Size, Alignment);
        }

        public void Deserialize(byte[] data)
        {
            object[] objects = Serializer.Deserialize(data, Size, Alignment);
            Size = (uint)objects[0];
            Alignment = (uint)objects[1];
        }
    }
}

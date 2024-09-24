using sK8.Serialization;

namespace sK8.Renderware.Resource
{
    /**
     * <summary>The base class for the platform dependent resource descriptors.</summary>
     */
    internal abstract class BaseResourceDescriptors : ISerializable
    {
        internal BaseResourceDescriptor[] Descriptors { get; }

        protected BaseResourceDescriptors(int numDescriptors)
        {
            Descriptors = new BaseResourceDescriptor[numDescriptors];
            for (int i = 0; i < numDescriptors; i++)
            {
                Descriptors[i] = new BaseResourceDescriptor(0, 1);
            }
        }

        internal uint GetTotalResourcesSize()
        {
            uint total = 0;
            foreach (BaseResourceDescriptor descriptor in Descriptors)
            {
                total += descriptor.Size;
            }
            return total;
        }

        public uint GetBufferSize()
        {
            return (uint)Descriptors.Length * 8;
        }

        public byte[] Serialize()
        {
            List<byte> bytes = new List<byte>();
            foreach (BaseResourceDescriptor descriptor in Descriptors)
            {
                bytes.AddRange(descriptor.Serialize());
            }
            return bytes.ToArray();
        }

        public void Deserialize(byte[] data)
        {
            byte[][] bytes = data.Chunk(8).ToArray();
            for (int i = 0; i < Descriptors.Length; i++)
            {
                Descriptors[i].Deserialize(bytes[i]);
            }
        }
    }
}

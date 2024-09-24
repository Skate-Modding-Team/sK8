using sK8.Serialization;

namespace sK8.Pegasus
{
    public class AttribPair : ISerializable
    {
        private ulong ClassKey;
        private ulong InstanceKey;


        
        public uint GetBufferSize()
        {
            return 16;
        }

        public byte[] Serialize()
        {
            return Serializer.Serialize(ClassKey, InstanceKey);
        }

        public void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, ClassKey, InstanceKey);
            ClassKey = (ulong)vals[0];
            InstanceKey = (ulong)vals[1];
        }
    }
}

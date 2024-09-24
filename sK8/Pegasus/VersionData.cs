using sK8.Renderware;
using sK8.Renderware.Arena;
using sK8.Serialization;

namespace sK8.Pegasus
{
    /**
     * <summary>
     * Pegasus Version Data:
     * Provides the version of pegasus used for the serialized objects in an arena.
     * This object is required for an arena and is always first.
     * </summary>
     */
    public class VersionData : IRwObject
    {
        private uint Version = 0;
        private uint Revision = 0;

        public VersionData(uint Vers, uint Rev)
        {
            Version = Vers;
            Revision = Rev;
        }

        public VersionData(byte[] Bytes)
        {
            Deserialize(Bytes);
        }

        public uint GetBufferSize()
        {
            return 8;
        }

        public ERwObjectType GetTypeID()
        {
            return ERwObjectType.PEGASUS_VERSIONDATA;
        }

        public byte[] Serialize()
        {
            return Serializer.Serialize(Version, Revision);
        }

        public void Deserialize(byte[] bytes)
        {
            object[] vals = Serializer.Deserialize(bytes, Version, Revision);
            Version = (uint)vals[0];
            Revision = (uint)vals[1];
        }
    }
}

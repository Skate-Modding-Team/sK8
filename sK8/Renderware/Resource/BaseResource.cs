using sK8.Renderware.Arena;

namespace sK8.Renderware.Resource
{
    /** <summary>
    *Used to treat resources as rw objects for arenas
    *</summary>
    */
    internal class BaseResource : IRwObject
    {
        private byte[] resource;

        internal BaseResource(byte[] resource)
        {
            this.resource = resource;
        }

        public ERwObjectType GetTypeID()
        {
            return ERwObjectType.RW_CORE_BASERESOURCE_0;
        }

        public uint GetBufferSize()
        {
            return (uint)resource.Length;
        }

        public byte[] Serialize()
        {
            return resource;
        }

        public void Deserialize(byte[] data)
        {
            resource = data;
        }
    }
}

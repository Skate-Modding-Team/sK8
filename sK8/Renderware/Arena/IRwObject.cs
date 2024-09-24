using sK8.Serialization;

namespace sK8.Renderware.Arena
{
    public interface IRwObject : ISerializable
    {
        /**<summary>returns the RW object type ID</summary>*/
        public ERwObjectType GetTypeID();
    }
}

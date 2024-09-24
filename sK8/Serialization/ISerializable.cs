using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sK8.Serialization
{
    public interface ISerializable
    {
        /**<summary>returns the size of the serialized object in number of bytes</summary>*/
        public uint GetBufferSize();
        /**<summary>serializes the rwobject to bytes</summary>*/
        public byte[] Serialize();
        /**<summary>fills out this rwobject based on the provided byte array</summary>*/
        public void Deserialize(byte[] bytes);
    }
}

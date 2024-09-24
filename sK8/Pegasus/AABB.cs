using sK8.Serialization;

namespace sK8.Pegasus
{
    /**<summary>
     * Bounding Box type used by pegasus.
     * </summary>
     */
    public class AABB : ISerializable
    {
        public System.Numerics.Vector4 Min { get; set; }
        public System.Numerics.Vector4 Max { get; set; }

        public AABB()
        {
            Min = new System.Numerics.Vector4(0, 0, 0, 0);
            Max = new System.Numerics.Vector4(0, 0, 0, 0);
        }

        public AABB(System.Numerics.Vector4 min, System.Numerics.Vector4 max)
        {
            Min = min;
            Max = max;
        }

        public AABB(float x1, float y1, float z1, float w1, float x2, float y2, float z2, float w2)
        {
            Min = new System.Numerics.Vector4(x1, y1, z1, w1);
            Max = new System.Numerics.Vector4(y2, z2, w2, x2);
        }

        public AABB(byte[] data)
        {
            Deserialize(data);
        }

        public uint GetBufferSize()
        {
            return 32;
        }

        public void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, Min, Max);
            Min = (System.Numerics.Vector4)vals[0];
            Max = (System.Numerics.Vector4)vals[1];

        }

        public byte[] Serialize()
        {
            return Serializer.Serialize(Min, Max);
        }
    }
}

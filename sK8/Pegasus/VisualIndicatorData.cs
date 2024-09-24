using sK8.Renderware;
using sK8.Renderware.Arena;
using sK8.Serialization;

namespace sK8.Pegasus
{
    /**<summary>
    * The visual indicator object is used in skate 2 and 3 for 2d spline art in the 3d space.
    * </summary>
    */
    public class VisualIndicator
    {
        [Flags]
        public enum VIFlags
        {
            Billboard = 1,
            OpenForm = 2,
            ClosedForm = 4,
            PeriodicForm = 8,
            BillboardAndOpenForm = Billboard | OpenForm,
            BillboardAndClosedForm = Billboard | ClosedForm,
            BillboardAndPeriodicForm = Billboard | PeriodicForm
        }

        //serialized structure
        public ulong Guid { get; }
        public ulong GuidLocal { get; }
        public AABB BBox { get; }
        public VIFlags Flags { get; set; }
        public uint NumSegments { get; }
        internal uint SegmentsPtr = 0;
        //4byte padding here

        internal List<System.Numerics.Matrix4x4> Segments;

        public VisualIndicator()
        {
            Segments = new List<System.Numerics.Matrix4x4>();
        }

        public VisualIndicator(List<System.Numerics.Matrix4x4> segments)
        {
            Segments = segments;
        }


    }

    /**<summary>
     * The visual indicator data object acts as a means to serialize multiple visual indicators into a list as an rw object.
     * </summary>
     */
    public class VisualIndicatorData : IRwObject
    {
        private uint NumVIs = 0;
        private uint NumSegments = 0;
        private uint VIsPtr = 16;
        private uint SegmentsPtr = 16;

        private List<VisualIndicator> Indicators;

        public VisualIndicatorData()
        {
            Indicators = new List<VisualIndicator>();
        }

        public VisualIndicatorData(byte[] data)
        {
            Deserialize(data);
        }

        public uint GetBufferSize()
        {
            return 16 + (uint)Indicators.Count * 128;
        }

        public ERwObjectType GetTypeID()
        {
            return ERwObjectType.PEGASUS_VISUALINDICATORDATA;
        }

        public byte[] Serialize()
        {
            byte[] data = new byte[GetBufferSize()];

            using (BufferWriter buffer = new BufferWriter(new MemoryStream(data)))
            {
                buffer.WriteBE(NumVIs);
                buffer.WriteBE(NumSegments);
                buffer.WriteBE(VIsPtr);
                buffer.WriteBE(SegmentsPtr);

                buffer.WritePadding((int)(VIsPtr - buffer.BaseStream.Position));

                foreach (VisualIndicator indicator in Indicators)
                {
                    buffer.WriteBE(indicator.Guid);
                    buffer.WriteBE(indicator.GuidLocal);
                    buffer.Write(indicator.BBox.Serialize());
                    buffer.WriteBE((int)indicator.Flags);
                    buffer.WriteBE(indicator.NumSegments);
                    buffer.WriteBE(indicator.SegmentsPtr);
                    buffer.WriteBE(0);
                }

                buffer.WritePadding((int)(SegmentsPtr - buffer.BaseStream.Position));

                foreach (VisualIndicator indicator in Indicators)
                {
                    foreach (System.Numerics.Matrix4x4 mat in indicator.Segments)
                    {
                        buffer.WriteBE(mat.M11);
                        buffer.WriteBE(mat.M12);
                        buffer.WriteBE(mat.M13);
                        buffer.WriteBE(mat.M14);
                        buffer.WriteBE(mat.M21);
                        buffer.WriteBE(mat.M22);
                        buffer.WriteBE(mat.M23);
                        buffer.WriteBE(mat.M24);
                        buffer.WriteBE(mat.M31);
                        buffer.WriteBE(mat.M32);
                        buffer.WriteBE(mat.M33);
                        buffer.WriteBE(mat.M34);
                        buffer.WriteBE(mat.M41);
                        buffer.WriteBE(mat.M42);
                        buffer.WriteBE(mat.M43);
                        buffer.WriteBE(mat.M44);
                    }
                }

                buffer.Flush();
            }

            return data;
        }

        public void Deserialize(byte[] bytes)
        {
            Indicators = new List<VisualIndicator>();

            using (BufferReader buffer = new BufferReader(new MemoryStream(bytes)))
            {
                NumVIs = buffer.ReadUInt32BE();
                NumSegments = buffer.ReadUInt32BE();
                VIsPtr = buffer.ReadUInt32BE();
                SegmentsPtr = buffer.ReadUInt32BE();


            }
        }

        public uint GetNumVisualIndicators()
        {
            return NumVIs;
        }

        public uint GetNumSegments()
        {
            return NumSegments;
        }

        public void AddVisualIndicator(VisualIndicator visualIndicator)
        {
            SegmentsPtr += 64;
            visualIndicator.SegmentsPtr = SegmentsPtr + 64 * NumSegments;
            NumVIs++;
            NumSegments += visualIndicator.NumSegments;
            Indicators.Add(visualIndicator);
        }
    }
}

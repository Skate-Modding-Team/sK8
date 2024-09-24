using sK8.Serialization;

namespace sK8.Renderware.Arena
{
    internal class ArenaSectionManifiest : ArenaSection
    {
        private readonly uint Dict = 12;

        internal uint[] SectionPtrs { get; }

        internal ArenaSectionManifiest() : base(SectionType.MANIFEST)
        {
            SectionPtrs = new uint[(int)SectionType.NUMBEROFSECTIONS - 1];
            base.NumEntries = 4;
        }

        public override uint GetBufferSize()
        {
            return 28;
        }

        public override byte[] Serialize()
        {
            return Serializer.Serialize((int)base.GetSectionType(), base.NumEntries, Dict, SectionPtrs[0], SectionPtrs[1], SectionPtrs[2], SectionPtrs[3]);
        }

        public override void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, (int)base.GetSectionType(), base.NumEntries, Dict, SectionPtrs[0], SectionPtrs[1], SectionPtrs[2], SectionPtrs[3]);
            base.NumEntries = (uint)vals[1];
            SectionPtrs[0] = (uint)vals[3];
            SectionPtrs[1] = (uint)vals[4];
            SectionPtrs[2] = (uint)vals[5];
            SectionPtrs[3] = (uint)vals[6];
        }
    }
}

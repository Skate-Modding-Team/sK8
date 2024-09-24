using sK8.Serialization;

namespace sK8.Renderware.Arena
{
    internal class ArenaSectionAtoms : ArenaSection
    {
        private uint AtomTable = 0;
        internal ArenaSectionAtoms() : base(SectionType.ATOMS) { }

        public override uint GetBufferSize()
        {
            return 12;
        }

        public override byte[] Serialize()
        {
            return Serializer.Serialize((int)base.GetSectionType(), base.NumEntries, AtomTable);
        }

        public override void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, (int)base.GetSectionType(), base.NumEntries, AtomTable);
            base.NumEntries = (uint)vals[1];
            AtomTable = (uint)vals[2];
        }
    }
}

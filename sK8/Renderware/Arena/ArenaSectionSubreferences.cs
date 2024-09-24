using sK8.Serialization;

namespace sK8.Renderware.Arena
{
    internal class ArenaSectionSubreferencesRecord : ISerializable
    {
        internal uint ObjectId;
        internal uint Offset;

        internal ArenaSectionSubreferencesRecord() {}

        internal ArenaSectionSubreferencesRecord(uint ObjectId, uint Offset)
        {
            this.ObjectId = ObjectId;
            this.Offset = Offset;
        }

        public uint GetBufferSize()
        {
            return 8;
        }

        public byte[] Serialize()
        {
            return Serializer.Serialize(ObjectId, Offset);
        }

        public void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, ObjectId, Offset);
            ObjectId = (uint)vals[0];
            Offset = (uint)vals[1];
        }
    }

    internal class ArenaSectionSubreferences : ArenaSection
    {
        private readonly uint DictAfterRefix = 0;
        private readonly uint RecordsAfterRefix = 0;
        internal uint Dict;
        internal uint Records;
        private uint NumUsed = 0;

        private List<ArenaSectionSubreferencesRecord> Subreferences = new List<ArenaSectionSubreferencesRecord>();

        internal ArenaSectionSubreferences() : base(SectionType.SUBREFERENCES) { }

        internal ArenaSectionSubreferencesRecord[] GetSubreferences()
        {
            return Subreferences.ToArray();
        }

        internal void AddSubreferenceRecord(ArenaSectionSubreferencesRecord record)
        {
            Subreferences.Add(record);
            NumUsed++;
            base.NumEntries++;
            Dict += record.GetBufferSize();
        }

        public override uint GetBufferSize()
        {
            return 28;
        }

        public override byte[]  Serialize()
        {
            return Serializer.Serialize((int)base.GetSectionType(), base.NumEntries, DictAfterRefix, RecordsAfterRefix, Dict, Records, NumUsed);
        }

        public override void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, (int)base.GetSectionType(), base.NumEntries, DictAfterRefix, RecordsAfterRefix, Dict, Records, NumUsed);
            base.NumEntries = (uint)vals[1];
            Dict = (uint)vals[4];
            Records = (uint)vals[5];
            NumUsed = (uint)vals[6];
        }
    }
}

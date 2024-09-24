using sK8.Serialization;

namespace sK8.Renderware.Arena
{
    //This seems to allow other Arenas or other Arenas' objects to be referenced in this arena, but I've never seen it used so I will leave it like this.
    internal class ArenaSectionExternalArenas : ArenaSection
    {
        private readonly uint Dict = 24;
        internal uint ArenaID;
        private readonly uint Unknown = 4289724416;



        internal ArenaSectionExternalArenas() : base(SectionType.EXTERNALARENAS)
        {
            base.NumEntries = 3;
        }

        public override uint GetBufferSize()
        {
            return 36;
        }

        public override byte[] Serialize()
        {
            return Serializer.Serialize((int)base.GetSectionType(), base.NumEntries, Dict, ArenaID, Unknown, ArenaID, 0, 0, 0);
        }

        public override void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, (int)base.GetSectionType(), base.NumEntries, Dict, ArenaID, Unknown, ArenaID, 0, 0, 0);
            ArenaID = (uint)vals[3];
        }
    }
}

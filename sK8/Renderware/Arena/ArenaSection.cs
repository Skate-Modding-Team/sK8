using sK8.Serialization;

namespace sK8.Renderware.Arena
{
    internal enum SectionType
    {
        MANIFEST = 0x00010004,
        TYPES = 0x00010005,
        EXTERNALARENAS = 0x00010006,
        SUBREFERENCES = 0x00010007,
        ATOMS = 0x00010008,
        NUMBEROFSECTIONS = 5
    }

    internal abstract class ArenaSection : ISerializable
    {
        private SectionType Type;
        protected uint NumEntries;

        internal ArenaSection(SectionType type)
        {
            Type = type;
            NumEntries = 0;
        }

        public SectionType GetSectionType()
        {
            return Type;
        }

        public virtual uint GetBufferSize()
        {
            return 8;
        }

        public virtual byte[] Serialize()
        {
            return Serializer.Serialize((int)Type, NumEntries);
        }

        public virtual void Deserialize(byte[] data)
        {
            object[] vals = Serializer.Deserialize(data, (int)Type, NumEntries);
        }
    }
}

using sK8.Serialization;

namespace sK8.Renderware.Arena
{
    internal class ArenaSectionTypes : ArenaSection
    {
        private readonly uint Dict = 12;

        private List<ERwObjectType> Types = new List<ERwObjectType>();

        internal ArenaSectionTypes() : base(SectionType.TYPES)
        {
            AddType(ERwObjectType.NA);
        }

        internal bool AddType(ERwObjectType type)
        {
            if (!Types.Contains(type))
            {
                Types.Add(type);
                base.NumEntries++;
                return true;
            }
            return false;
        }

        internal void RemoveType(ERwObjectType type)
        {
            if (Types.Contains(type))
            {
                Types.Remove(type);
                base.NumEntries--;
            }
        }

        internal bool ContainsType(ERwObjectType type)
        {
            return Types.Contains(type);
        }

        internal uint GetTypeIndex(ERwObjectType type)
        {
            return (uint)Types.IndexOf(type);
        }

        public override uint GetBufferSize()
        {
            return 12 + ((uint)Types.Count*4);
        }

        public override byte[] Serialize()
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(Serializer.Serialize((int)base.GetSectionType(), base.NumEntries, Dict));

            foreach (ERwObjectType type in Types)
            {
                bytes.AddRange(Serializer.Serialize((int)type));
            }

            return bytes.ToArray();
        }

        public override void Deserialize(byte[] data)
        {
            Types = new List<ERwObjectType>();

            object[] vals = Serializer.Deserialize(data, (int)base.GetSectionType(), base.NumEntries);
            base.NumEntries = (uint)vals[1];

            byte[][] typeByteArray = data.TakeLast(data.Length - 12).Chunk(4).ToArray();

            foreach (byte[] typeBytes in typeByteArray)
            {
                Types.Add((ERwObjectType)Serializer.Deserialize(typeBytes, 0)[0]);
            }
        }
    }
}

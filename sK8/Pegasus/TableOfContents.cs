using sK8.Renderware;
using sK8.Renderware.Arena;
using sK8.Serialization;

namespace sK8.Pegasus
{
    internal record TOCEntry(uint NamePtr, ulong Guid, ERwObjectType Type, uint ObjectPtr); //NOTE: 4-byte padding after name ptr

    internal record TypeMapEntry(ERwObjectType Type, uint Index);

    /**
     * <summary>
     * Pegasus Table of Contents Object
     * This provides a Table of contents for the pegasus objects in the arena file which the internal pegasus system of skate uses.
     * This object is required in all arenas and is always last.
     * Ideally, this can be auto-generated and appended to an arena for completion.
     * </summary>
     */
    public class TableOfContents : IRwObject
    {
        private uint ItemsCount = 0;
        private uint EntryArrayPtr = 20;
        private uint NamesPtr = 20;
        private uint TypeCount = 0;
        private uint TypeMapPtr = 20;

        private List<TOCEntry> TOCEntries = new List<TOCEntry>();
        private List<string> Names = new List<string>();
        private List<TypeMapEntry> TypeMapEntries = new List<TypeMapEntry>();

        public TableOfContents() { }

        public TableOfContents(byte[] data)
        {
            Deserialize(data);
        }

        public ERwObjectType GetTypeID()
        {
            return ERwObjectType.PEGASUS_TABLEOFCONTENTS;
        }
        
        public uint GetBufferSize()
        {
            uint strSize = 0;
            foreach(string str in Names)
            {
                strSize += (uint)str.Length + 1;
            }

            return EntryArrayPtr + (uint)TOCEntries.Count*24 + (uint)TypeMapEntries.Count*8 + strSize;
        }

        public byte[] Serialize()
        {
            byte[] bytes = new byte[GetBufferSize()];

            using (BufferWriter buffer = new BufferWriter(new MemoryStream(bytes)))
            {
                //Header
                buffer.WriteBE(ItemsCount);
                buffer.WriteBE(EntryArrayPtr);
                buffer.WriteBE(NamesPtr);
                buffer.WriteBE(TypeCount);
                buffer.WriteBE(TypeMapPtr);

                //Padding Just in Case
                buffer.WritePadding((int)EntryArrayPtr - 20);

                //TOC Entries
                foreach (TOCEntry entry in TOCEntries)
                {
                    buffer.WriteBE(entry.NamePtr);
//                    if (entry.Type == ERwObjectType.PEGASUS_TEXTURE) //NOT SURE WHAT THIS DOES. WILL NEED TO BE RESEARCHED IN THE FUTURE. DEBUG SYMBOLS SAY IT IS JUST PADDING.
//                    {
//                        buffer.WriteBE(0x7C0F1678);
//                    }
//                    else
                    {
                        buffer.WriteBE(0xFEFFFFFF);
                    }
                    buffer.WriteBE(entry.Guid);
                    buffer.WriteBE((int)entry.Type);
                    buffer.WriteBE(entry.ObjectPtr);
                }

                //Padding Just in Case
                buffer.WritePadding((int)(NamesPtr - buffer.BaseStream.Position));
                
                //Names
                foreach (string name in Names)
                {
                    buffer.WriteNTString(name);
                }

                //Padding Just in Case
                buffer.WritePadding((int)(TypeMapPtr - buffer.BaseStream.Position));

                //Types
                foreach (TypeMapEntry entry in  TypeMapEntries)
                {
                    buffer.Write((int)entry.Type);
                    buffer.WriteBE(entry.Index);
                }

                buffer.Flush();
            }

            return bytes;
        }

        public void Deserialize(byte[] bytes)
        {
            TOCEntries = new List<TOCEntry>();
            Names = new List<string>();
            TypeMapEntries = new List<TypeMapEntry>();

            try
            {
                using (BufferReader buffer = new BufferReader(new MemoryStream(bytes)))
                {
                    uint numNames = 0;

                    //Header
                    ItemsCount = buffer.ReadUInt32BE();
                    EntryArrayPtr = buffer.ReadUInt32BE();
                    NamesPtr = buffer.ReadUInt32BE();
                    TypeCount = buffer.ReadUInt32BE();
                    TypeMapPtr = buffer.ReadUInt32BE();

                    //TOCEntries
                    buffer.BaseStream.Seek(EntryArrayPtr, SeekOrigin.Begin);

                    for (int i = 0; i < ItemsCount; i++)
                    {
                        uint nptr = buffer.ReadUInt32BE(); //this is done so we can skip the "padding" bytes
                        buffer.ReadUInt32BE();
                        TOCEntry entry = new TOCEntry(
                            nptr,
                            buffer.ReadUInt64BE(),
                            (ERwObjectType)buffer.ReadInt32BE(),
                            buffer.ReadUInt32BE());

                        if (entry.NamePtr != 0)
                        {
                            numNames++;
                        }

                        TOCEntries.Add(entry);
                    }

                    //Names
                    buffer.BaseStream.Seek(NamesPtr, SeekOrigin.Begin);

                    for (int i = 0; i < numNames; i++)
                    {
                        Names.Add(buffer.ReadNTString());
                    }

                    //TypeMap
                    buffer.BaseStream.Seek(TypeMapPtr, SeekOrigin.Begin);

                    for (int i = 0; i < TypeCount; i++)
                    {
                        TypeMapEntry entry = new TypeMapEntry(
                            (ERwObjectType)buffer.ReadInt32(),
                            buffer.ReadUInt32BE());

                        TypeMapEntries.Add(entry);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Buffer Reading Failed!");
            }
        }

        public override string ToString()
        {
            string str = $"Number of Items: {ItemsCount}\n";

            for (int i = 0; i < TOCEntries.Count; i++)
            {
                str += $"Object #{i + 1}\n";
                str += $"\tType: {Enum.GetName(typeof(ERwObjectType), TOCEntries[i].Type)}\n";
                str += $"\tGUID: {BitConverter.ToString(BitConverter.GetBytes(TOCEntries[i].Guid).Reverse().ToArray()).Replace("-","")}\n";
                if (TOCEntries[i].ObjectPtr >= 8388608)
                {
                    str += $"\tSubrefIndex: {TOCEntries[i].ObjectPtr - 8388607}\n\n";
                }
                else
                {
                    str += $"\tArenaIndex: {TOCEntries[i].ObjectPtr}\n\n";
                }
                
            }

            return str;
        }

        //Ideally the Table of Contents will be auto generated when building an arena, so there isn't much list functionality here for now.
    }
}

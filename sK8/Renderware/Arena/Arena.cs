using sK8.Renderware.Resource;
using sK8.Serialization;

namespace sK8.Renderware.Arena
{

    /**
     * <summary>
     * Each serialized object in the Arena gets an entry to represent it in the Arena dictionary.
     * </summary>
     * 
     * <remarks>
     * TypeIndex refers to the index of the object's type in the ArenaSectionTypes object.
     * TypeID refers to the actual object's type ID.
     * </remarks>
     */
    public record struct ArenaDictionaryEntry(uint Pointer, uint Reloc, uint Size, uint Align, uint TypeIndex, ERwObjectType TypeID)
    {
        public byte[] Serialize()
        {
            return Serializer.Serialize(Pointer, Reloc, Size, Align, TypeIndex, (int)TypeID);
        }

        public static uint GetSize()
        {
            return 24;
        }
    }

    internal static class ArenaFileHeader
    {
        //Magic Number
        internal static readonly uint Prefix = 2303874868;
        //body is EPlatform
        internal static readonly uint Suffix = 218765834;
        //End Magic Number

        //File Header
        internal static readonly bool IsBigEndian = true;
        internal static readonly byte PointerSizeInBits = 32;
        internal static readonly byte PointerAlignment = 4;
        //unused byte here

        internal static readonly string MajorVersion = "454";
        internal static readonly string MinorVersion = "000";
        internal static readonly uint BuildNumber = 0;

        internal static byte[] Serialize(EPlatform platform)
        {
            return Serializer.Serialize(Prefix, (int)platform, Suffix, IsBigEndian, PointerSizeInBits, PointerAlignment, (byte)0, MajorVersion, MinorVersion, BuildNumber);
        }
    }

    /**
     * <summary>
     * Represents a skate-specific RW4 Arena file.
     * 
     * Arenas are a collection of serialized RW objects that can be loaded into memory via the one arena file.
     * </summary>
     */
    public class Arena : ISerializable
    {
        private EPlatform Platform;

        private uint Id;
        private uint NumEntries = 0;
        private uint NumUsed = 0;
        private uint Alignment; //Every new memory resource in the resource descriptor should be serialized starting at a multiple of this value. Wii uses 32, Ps3 and Xbox use 16.
                                      //Each object or entry within that resource is then aligned based on that resource's alignment value.
                                      //Each entry within an object is then aligned based on that entry's alignment in the arena dictionary entry
        private uint DictStart;
        private uint Sections;

        private BaseResourceDescriptors ResourceDescriptor; //Main Memory of this is the header plus rw objects plus dictionary and subreference records
        private BaseResourceDescriptors ResourcesUsed; //Main Memory of this is the Arena Header

        private ArenaSectionManifiest SectionManifiest = new ArenaSectionManifiest();
        private ArenaSectionTypes SectionTypes = new ArenaSectionTypes();
        private ArenaSectionExternalArenas SectionExternalArenas = new ArenaSectionExternalArenas();
        private ArenaSectionSubreferences SectionSubreferences = new ArenaSectionSubreferences();
        private ArenaSectionAtoms SectionAtoms = new ArenaSectionAtoms();

        private List<IRwObject> Objects = new List<IRwObject>();
        private List<ArenaDictionaryEntry> DictionaryEntries = new List<ArenaDictionaryEntry>();

        public Arena(EPlatform platform, string name = "Arena")
        {
            Platform = platform;
            Id = RwHash.RwHash32String(name);

            switch (Platform)
            {
                case EPlatform.WII:
                    Alignment = 32;
                    Sections = 152;
                    ResourceDescriptor = new RevDescriptors();
                    ResourcesUsed = new RevDescriptors();
                    ResourcesUsed.Descriptors[(int)RevResources.MAINMEMORY] = new BaseResourceDescriptor(152, 4); //Header Memory Descriptor

                    //initialize resource descriptor types
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_0);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_1);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_2);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_3);
                    break;

                case EPlatform.PS3:
                    Alignment = 16;
                    Sections = 192;
                    ResourceDescriptor = new Ps3Descriptors();
                    ResourcesUsed = new Ps3Descriptors();
                    ResourcesUsed.Descriptors[(int)Ps3Resources.MAINMEMORY] = new BaseResourceDescriptor(192, 4); //Header Memory Descriptor

                    //initialize resource descriptor types
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_0);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_1);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_2);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_3);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_4);
                    break;

                case EPlatform.Xenon:
                    Alignment = 16;
                    Sections = 172;
                    ResourceDescriptor = new Xbox2Descriptors();
                    ResourcesUsed = new Xbox2Descriptors();
                    ResourcesUsed.Descriptors[(int)Xbox2Resources.MAINMEMORY] = new BaseResourceDescriptor(172, 4); //Header Memory Descriptor

                    //initialize resource descriptor types
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_0);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_1);
                    SectionTypes.AddType(ERwObjectType.RW_CORE_BASERESOURCE_2);
                    break;

                default:
                    throw new ArgumentException("Invalid Platform Supplied for Arena!");
            }
            //initialize global types
            SectionTypes.AddType(ERwObjectType.RW_CORE_ARENALOCALATOMTABLE);
            SectionTypes.AddType(ERwObjectType.RW_CORE_ARENA);
            SectionTypes.AddType(ERwObjectType.RW_CORE_RAW);
            SectionTypes.AddType(ERwObjectType.RW_CORE_BITTABLE);

            //Adjust the Main Memory Resource Descriptor and initialize some offsets
            uint startSize = ResourcesUsed.Descriptors[0].Size + GetSectionSizes();
            ResourceDescriptor.Descriptors[0] = new BaseResourceDescriptor(startSize, Alignment);

            DictStart = ResourceDescriptor.Descriptors[0].Size;
            SectionSubreferences.Dict = DictStart;
            SectionSubreferences.Records = DictStart;

            UpdateSectionManifest();
        }

        private void UpdateSectionManifest()
        {
            SectionManifiest.SectionPtrs[0] = SectionManifiest.GetBufferSize();
            SectionManifiest.SectionPtrs[1] = SectionManifiest.SectionPtrs[0] + SectionTypes.GetBufferSize();
            SectionManifiest.SectionPtrs[2] = SectionManifiest.SectionPtrs[1] + SectionExternalArenas.GetBufferSize();
            SectionManifiest.SectionPtrs[3] = SectionManifiest.SectionPtrs[2] + SectionSubreferences.GetBufferSize();
        }

        private uint GetSectionSizes()
        {
            return SectionManifiest.GetBufferSize() + SectionTypes.GetBufferSize() + SectionExternalArenas.GetBufferSize() + SectionSubreferences.GetBufferSize() + SectionAtoms.GetBufferSize();
        }

        private uint GetPaddingRequirement(uint size, uint alignment)
        {
            if (size%alignment == 0)
                return 0;
            
            return alignment - (size%alignment);
        }

        public uint GetId()
        {
            return Id;
        }
        
        public void AddObject(IRwObject obj)
        {
            //TODO: Change this to a switch on the type and add any necessary subreferences!
            if (obj.GetTypeID() == ERwObjectType.PEGASUS_VERSIONDATA && Objects.Any(x => x.GetTypeID() == ERwObjectType.PEGASUS_VERSIONDATA))
            {
                throw new ArgumentException("There is already a Version Data object in the arena!");
            }
            if (obj.GetTypeID() == ERwObjectType.PEGASUS_TABLEOFCONTENTS && Objects.Any(x => x.GetTypeID() == ERwObjectType.PEGASUS_TABLEOFCONTENTS))
            {
                throw new ArgumentException("There is already a Table of Contents object in the arena!");
            }

            if(SectionTypes.AddType(obj.GetTypeID()))
            {
                DictStart += 4;
                SectionSubreferences.Records += 4;
                SectionSubreferences.Dict += 4;
                ResourceDescriptor.Descriptors[0].Size += 4;
            }

            uint extraSize = GetPaddingRequirement(DictStart, Alignment);
            uint ptr = DictStart + extraSize;

            if (DictionaryEntries.Count > 0)
            {
                ptr = DictionaryEntries.Last().Pointer + DictionaryEntries.Last().Size;
                extraSize = GetPaddingRequirement(ptr, Alignment);
                ptr += extraSize;
            }

            ArenaDictionaryEntry dictEntry = new ArenaDictionaryEntry(ptr, 0, obj.GetBufferSize(), Alignment, SectionTypes.GetTypeIndex(obj.GetTypeID()), obj.GetTypeID());

            NumEntries++;
            NumUsed++;
            DictionaryEntries.Add(dictEntry);
            Objects.Add(obj);
            DictStart += extraSize + obj.GetBufferSize();
            SectionSubreferences.Records += ArenaDictionaryEntry.GetSize() + extraSize + obj.GetBufferSize();
            SectionSubreferences.Dict += ArenaDictionaryEntry.GetSize() + extraSize + obj.GetBufferSize();
            ResourceDescriptor.Descriptors[0].Size += extraSize + obj.GetBufferSize() + ArenaDictionaryEntry.GetSize();
        }

        public void AddSubreference(uint index, uint offset)
        {
            if (index >= DictionaryEntries.Count || offset > DictionaryEntries[(int)index].Size)
                throw new ArgumentException("Subreference Parameters are invalid!");

            SectionSubreferences.AddSubreferenceRecord(new ArenaSectionSubreferencesRecord(index, offset));
            ResourceDescriptor.Descriptors[0].Size += 8;
        }

        //Someday I'll make this work like the original rw Arena resource system where you can add each of the baseresourcedescriptor pieces of memory
        public void AddResource(byte[] resource, uint alignment = 0)
        {
            if (alignment == 0)
                alignment = Alignment;

            ERwObjectType type = ERwObjectType.RW_CORE_BASERESOURCE_0;

            uint currentResourceSize = 0;

            switch (Platform)
            {
                case EPlatform.PS3:
                    type = ERwObjectType.RW_CORE_BASERESOURCE_4; //Ps3Resources.GRAPHICS_LOCAL
                    currentResourceSize = ResourceDescriptor.Descriptors[(int)Ps3Resources.GRAPHICS_LOCAL].Size;
                    ResourceDescriptor.Descriptors[(int)Ps3Resources.GRAPHICS_LOCAL] = new BaseResourceDescriptor((uint)resource.Length + currentResourceSize + (currentResourceSize % 4), 4);
                    break;

                case EPlatform.Xenon:
                    type = ERwObjectType.RW_CORE_BASERESOURCE_1; //Xbox2Resources.PHYSICAL
                    currentResourceSize = ResourceDescriptor.Descriptors[(int)Xbox2Resources.PHYSICAL].Size;
                    ResourceDescriptor.Descriptors[(int)Xbox2Resources.PHYSICAL] = new BaseResourceDescriptor((uint)resource.Length + currentResourceSize + (currentResourceSize % 4), 4);
                    break;

                case EPlatform.WII:
                    throw new NotSupportedException("Wii doesn't use any resource besides Main Memory in Skate!");
            }

            ArenaDictionaryEntry dictEntry = new ArenaDictionaryEntry(currentResourceSize + GetPaddingRequirement(currentResourceSize, 4), 0, (uint)resource.Length, alignment, SectionTypes.GetTypeIndex(type), type);

            NumEntries++;
            NumUsed++;
            DictionaryEntries.Add(dictEntry);
            Objects.Add(new BaseResource(resource));
            SectionSubreferences.Records += ArenaDictionaryEntry.GetSize();
            SectionSubreferences.Dict += ArenaDictionaryEntry.GetSize();
            ResourceDescriptor.Descriptors[0].Size += ArenaDictionaryEntry.GetSize();
        }

        public IRwObject GetObject(uint id)
        {
            return Objects[(int)id];
        }

        public ArenaDictionaryEntry[] GetDictionary()
        {
            return DictionaryEntries.ToArray();
        }

        public void RemoveObject(uint id)
        {
            throw new NotImplementedException("Removing Objects is not yet supported!");
        }

        public sK8.Pegasus.VersionData GetVersionData()
        {
            return Objects.OfType<sK8.Pegasus.VersionData>().Single();
        }

        public sK8.Pegasus.TableOfContents GetPegasusTOC()
        {
            return Objects.OfType<sK8.Pegasus.TableOfContents>().Single();
        }

        public uint GetBufferSize()
        {
            return ResourceDescriptor.GetTotalResourcesSize() + ResourcesUsed.GetTotalResourcesSize();
        }

        public byte[] Serialize()
        {
            List<byte> serialized = new List<byte>();

            //fix External Arenas ID
            SectionExternalArenas.ArenaID = Id;

            //fix the end of our main memory if there are other resources
            uint extraBytes = GetPaddingRequirement(ResourceDescriptor.Descriptors[0].Size, Alignment);
            if (Objects.Any(x => x.GetTypeID() == ERwObjectType.RW_CORE_BASERESOURCE_0))
                ResourceDescriptor.Descriptors[0].Size += extraBytes;

            //Header
            serialized.AddRange(ArenaFileHeader.Serialize(Platform));
            serialized.AddRange(Serializer.Serialize(Id, NumEntries, NumUsed, Alignment, 0, DictStart, Sections, 0, 0, 0, ResourceDescriptor, ResourcesUsed, 0));

            //These are for Target Resource but it isnt even used, so I'm doing this.
            for (int i = 0; i < ResourceDescriptor.Descriptors.Length; i++)
                serialized.AddRange(Serializer.Serialize(0));
            //End Header

            //Sections
            serialized.AddRange(Serializer.Serialize(SectionManifiest, SectionTypes, SectionExternalArenas, SectionSubreferences, SectionAtoms));
            //End Sections

            // Objects
            for (int i = 0; i < NumEntries; i++)
            {
                if (Objects[i].GetTypeID() != ERwObjectType.RW_CORE_BASERESOURCE_0)
                {
                    //write padding
                    while (serialized.Count < DictionaryEntries[i].Pointer)
                    {
                        serialized.Add((byte)0);
                    }

                    serialized.AddRange(Objects[i].Serialize());
                }
            }
            // End Objects

            // Dictionary
            foreach (ArenaDictionaryEntry entry in DictionaryEntries)
            {
                serialized.AddRange(entry.Serialize());
            }
            // End Dictionary

            // Subreference Records
            foreach(ArenaSectionSubreferencesRecord record in SectionSubreferences.GetSubreferences())
            {
                serialized.AddRange(record.Serialize());
            }
            // End Subreference Records

            //add padding if needed
            if (Objects.Any(x => x.GetTypeID() == ERwObjectType.RW_CORE_BASERESOURCE_0))
                for (int i = 0; i < extraBytes; i++)
                    serialized.Add((byte)0);

            // Additional Resources
            if (Objects.Any(x => x.GetTypeID() == ERwObjectType.RW_CORE_BASERESOURCE_0))
            {
                for (int i = 0; i < NumEntries; i++)
                {
                    if (Objects[i].GetTypeID() == ERwObjectType.RW_CORE_BASERESOURCE_0)
                    {
                        //write padding
                        while (serialized.Count < ResourceDescriptor.Descriptors[0].Size + DictionaryEntries[i].Pointer)
                        {
                            serialized.Add((byte)0);
                        }

                        serialized.AddRange(Objects[i].Serialize());
                    }
                }
            }
            // End

            return serialized.ToArray();
        }

        public void Deserialize(byte[] data)
        {
             
        }
    }
}

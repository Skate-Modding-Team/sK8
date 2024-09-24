namespace sK8.Renderware
{
    /**<summary>
     * Class used to do the 32 bit and 64 bit rw Hashing algorithms.
     * </summary>
     */
    public static class RwHash
    {
        //default seeding values used in Skate (perhaps in general RW as well)
        private static readonly uint Hash32Seed = 0x811c9dc5;
        private static readonly ulong Hash64Seed = 0xcbf29ce484222325;

        //32 bit magic FNV-1 prime
        private static readonly uint Rw32Prime = 0x01000193;
        //64 bit magic FNV-1 prime
        private static readonly ulong Rw64Prime = 0x100000001b3;

        /////////////////
        //String Hashes//
        /////////////////
        public static ulong RwHash64String(string str)
        {
            ulong hash = Hash64Seed;

            //FNV-1 hash each byte
            foreach (byte b in str)
            {
                //multiply the seed by the prime
                hash *= Rw64Prime;

                //xor the bottom with the current byte
                hash ^= b;
            }

            return hash;
        }

        public static uint RwHash32String(string str)
        {
            uint hash = Hash32Seed;

            //FNV-1 hash each byte
            foreach (byte b in str)
            {
                //multiply the seed by the prime
                hash *= Rw32Prime;

                //xor the bottom with the current byte
                hash ^= b;
            }

            return hash;
        }

        /////////////////
        //Buffer Hashes//
        /////////////////
        public static ulong RwHash64Buffer(byte[] buffer)
        {
            ulong hash = Hash64Seed;

            foreach (byte b in buffer)
            {
                hash *= Rw64Prime;

                hash ^= b;
            }

            return hash;
        }

        public static uint RwHash32Buffer(byte[] buffer)
        {
            uint hash = Hash32Seed;

            foreach (byte b in buffer)
            {
                hash *= Rw32Prime;

                hash ^= b;
            }

            return hash;
        }
    }
}

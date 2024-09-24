using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sK8
{
    public static class DJB2Hash
    {
        public static uint Hash32(string str)
        {
            uint hash = 5381;

            foreach (byte b in str)
            {
                hash = ((hash << 5) + hash) + b;
            }

            return hash;
        }

        public static ulong Hash64(string str)
        {
            ulong hash = 5381;

            foreach (byte b in str)
            {
                hash = ((hash << 5) + hash) + b;
            }

            return hash;
        }
    }
}

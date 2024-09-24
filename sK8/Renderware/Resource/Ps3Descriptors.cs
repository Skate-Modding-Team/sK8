using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sK8.Renderware.Resource
{
    internal enum Ps3Resources
    {
        MAINMEMORY,
        DISPOSABLE,
        UNINITIALIZED,
        DISPOSABLE_UNINITIALIZED,
        GRAPHICS_SYSTEM,
        GRAPHICS_LOCAL, // Graphics base resources
        NUMBEROFBASERESOURCETYPES
    }

    internal class Ps3Descriptors : BaseResourceDescriptors
    {
        internal Ps3Descriptors() : base((int)Ps3Resources.NUMBEROFBASERESOURCETYPES) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sK8.Renderware.Resource
{
    internal enum RevResources
    {
        MAINMEMORY,
        DISPOSABLE,
        UNINITIALIZED,
        DISPOSABLE_UNINITIALIZED,
        NUMBEROFBASERESOURCETYPES
    }

    internal class RevDescriptors : BaseResourceDescriptors
    {
        internal RevDescriptors() : base((int)RevResources.NUMBEROFBASERESOURCETYPES) { }
    }
}

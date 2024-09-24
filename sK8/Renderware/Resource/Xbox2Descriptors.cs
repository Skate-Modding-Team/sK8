using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sK8.Renderware.Resource
{
    internal enum Xbox2Resources
    {
        MAINMEMORY,
        DISPOSABLE,
        PHYSICAL, //Graphics Base Resources
        UNINITIALIZED,
        DISPOSABLE_UNINITIALIZED,
        NUMBEROFBASERESOURCETYPES
    };

    internal class Xbox2Descriptors : BaseResourceDescriptors
    {
        internal Xbox2Descriptors() : base((int)Xbox2Resources.NUMBEROFBASERESOURCETYPES) { }
    }
}

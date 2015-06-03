using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaristaLabs.Espresso.Engine.V8
{
    /// <summary>
    /// Defines options for exposing host resources to script code.
    /// </summary>
    [Flags]
    public enum HostItemFlags
    {
        /// <summary>
        /// Specifies that no options are selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the host resource's members are to be exposed as global items in the
        /// script engine's root namespace.
        /// </summary>
        GlobalMembers = 0x00000001,

        /// <summary>
        /// Specifies that the host resource's non-public members are to be exposed.
        /// </summary>
        PrivateAccess = 0x00000002,

        /// <summary>
        /// Specifies that the host resource's dynamic members are not to be exposed. This option
        /// applies only to objects that implement <see cref="IDynamicMetaObjectProvider"/>.
        /// </summary>
        HideDynamicMembers = 0x00000004
    }
}

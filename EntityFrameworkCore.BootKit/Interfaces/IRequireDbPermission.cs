using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.BootKit
{
    /// <summary>
    /// Get permission before update record, it won't commit when any implementation return false.
    /// </summary>
    public interface IRequireDbPermission
    {
        /// <summary>
        /// Allow invoke patch method
        /// </summary>
        /// <param name="patch"></param>
        /// <returns></returns>
        Boolean AllowPatch(DbPatchModel patch);
    }
}

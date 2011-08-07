using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public enum SubCategoryType
    {
        /// <summary>
        /// Everybody can add items
        /// </summary>
        OpenList = 0,

        /// <summary>
        /// Only admins can change the list
        /// </summary>
        ClosedList = 1
    }
}

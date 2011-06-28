using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class SubCategory : Category
    {
        public virtual MainCategory MainCategory { get; set; }
    }
}

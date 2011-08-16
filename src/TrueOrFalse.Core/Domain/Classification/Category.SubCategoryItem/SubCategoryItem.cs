using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class SubCategoryItem : DomainEntity
    {
        /// <summary>
        /// Parent
        /// </summary>
        public virtual SubCategory SubCategory { get; set; }

        public virtual string Name { get; set; }

        public SubCategoryItem(){}

        public SubCategoryItem(string name)
        {
            Name = name;
        }
    }
}

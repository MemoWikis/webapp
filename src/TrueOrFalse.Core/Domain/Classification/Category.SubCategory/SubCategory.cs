using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class SubCategory : DomainEntity
    {
        public virtual Category Category { get; set; }
        public virtual IList<SubCategoryItem> Items { get; set; } 

        public virtual string Name { get; set; }
        public virtual SubCategoryType Type { get; set; }  

        public SubCategory()
        {
            Items = new List<SubCategoryItem>();
        }

        public SubCategory(string name) : this()
        {
            Name = name;
        }
    }
}

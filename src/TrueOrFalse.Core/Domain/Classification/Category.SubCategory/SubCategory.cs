using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class SubCategory : IPersistable, WithDateCreated
    {
        public virtual int Id { get; set; }

        public virtual Category Category { get; set; }
        public virtual IList<SubCategoryItem> Items { get; set; } 

        public virtual string Name { get; set; }
        public virtual SubCategoryType Type { get; set; }  

        public virtual DateTime DateModified { get; set; }
        public virtual DateTime DateCreated { get; set; }

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

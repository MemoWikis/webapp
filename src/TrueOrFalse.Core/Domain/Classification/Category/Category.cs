using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class Category : IPersistable, WithDateCreated
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime DateModified { get; set; }
        public virtual DateTime DateCreated { get; set; }

        public virtual IList<Classification> Classifications { get; set; }

        public Category()
        {
            Classifications = new List<Classification>();
        }

        public Category(string name) : this()
        {
            Name = name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class ClassificationItem : IPersistable, WithDateCreated
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// Parent
        /// </summary>
        public virtual Classification Classification { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime DateModified { get; set; }
        public virtual DateTime DateCreated { get; set; }

        public ClassificationItem(){}

        public ClassificationItem(string name)
        {
            Name = name;
        }
    }
}

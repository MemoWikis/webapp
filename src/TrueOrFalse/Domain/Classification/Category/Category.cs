using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    [Serializable]
    public class Category : DomainEntity
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string WikipediaURL { get; set; }

        public virtual User Creator { get; set; }
        public virtual IList<Category> ParentCategories { get; set; }
        
        public virtual int CountQuestions { get; set; }
        public virtual int CountSets { get; set; }
        public virtual int CountCreators { get; set; }

        public Category(){
            ParentCategories = new List<Category>();
        }

        public Category(string name) : this(){
            Name = name;
        }
    }
}
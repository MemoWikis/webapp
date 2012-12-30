using System.Collections.Generic;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class Category : DomainEntity
    {
        public virtual string Name { get; set; }
        public virtual User Creator { get; set; }
        public virtual IList<Category> RelatedCategories { get; set; }
        public virtual IList<Question> Questions { get; set; }

        public Category(){
            RelatedCategories = new List<Category>();
        }

        public Category(string name) : this(){
            Name = name;
        }
    }
}
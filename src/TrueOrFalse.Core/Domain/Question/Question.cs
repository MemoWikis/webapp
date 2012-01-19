using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core
{
    public class Question : DomainEntity
    {
        
        public virtual string Text { get; set; }
        public virtual string Description { get; set; }
        public virtual string Solution { get; set; }
        public virtual IList<Category> Categories { get; set; }
        public virtual QuestionVisibility Visibility { get; set; }
        public virtual SolutionType SolutionType { get; set; }
        public virtual User Creator { get; set; }

        public Question()
        {
            Categories = new List<Category>();
        }

        public virtual string GetShortTitle()
        {
            return Text.TruncateAtWord(96);
        }

    }
}

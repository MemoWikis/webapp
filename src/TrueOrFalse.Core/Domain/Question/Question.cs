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
        public virtual IList<Answer> Answers { get; set; }
        public virtual QuestionVisibility Visibility { get; set; }
        public virtual User Creator { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }

        public virtual string GetShortTitle()
        {
            return Text.TruncateAtWord(96);
        }

    }
}

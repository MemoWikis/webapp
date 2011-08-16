using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core
{
    public class Question : IPersistable, WithDateCreated
    {
        public virtual int Id{ get; set;}
  
        public virtual string Text { get; set; }

        public virtual IList<Answer> Answers { get; set; }

        public virtual QuestionVisibility Visibility { get; set; }

        public virtual DateTime DateModified { get; set; }
        public virtual DateTime DateCreated { get; set; }

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

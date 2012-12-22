using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse
{
    public class QuestionSet : DomainEntity
    {
        public virtual string Name { get; set; }
        public virtual string Text { get; set; }

        public virtual IList<Question> Questions { get; set; }
        public virtual User Creator { get; set; }

        public virtual void Add(IList<Question> questions){
            foreach (var question in questions)
                Questions.Add(question);
        }

        public QuestionSet(){
            Questions = new List<Question>();
        }
    }
}

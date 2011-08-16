using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class Answer : DomainEntity
    {
        public Answer(){}

        public Answer(string answerText)
        {
            Text = answerText;
        }

        public virtual Question Question { get; set; }

        public virtual string Text { get; set; }
        public virtual string Description { get; set; }
        public virtual AnswerType Type { get; set; }
    }
}

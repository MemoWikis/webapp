using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{

    public class Answer : IPersistable, WithDateCreated
    {
        public Answer(){}

        public Answer(string answerText)
        {
            Text = answerText;
        }

        public virtual int Id { get; set; }

        public virtual string Text { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateModified { get; set; }
    }
}

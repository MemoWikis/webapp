using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class Question : IPersistable, WithDateCreated
    {
        public virtual int Id{ get; set;}

        public virtual Answer Answer{ get; set; }
        public virtual string Text { get; set; }

        public virtual bool IsValidAnswer(UserInputText userinput)
        {
            return Answer.Text.Equals(userinput.Text);
        }

        public virtual DateTime DateModified { get; set; }
        public virtual DateTime DateCreated { get; set; }
    }
}

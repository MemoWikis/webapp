using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class Question
    {
        public Answer Answer; 

        public bool IsValidAnswer(UserInputText userinput)
        {
            return Answer.Text.Equals(userinput.Text);
        }
    }
}

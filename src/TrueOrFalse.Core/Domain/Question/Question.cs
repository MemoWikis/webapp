using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class Question
    {
        public int Id;

        public Answer Answer;
        public string Text;

        public bool IsValidAnswer(UserInputText userinput)
        {
            return Answer.Text.Equals(userinput.Text);
        }
    }
}

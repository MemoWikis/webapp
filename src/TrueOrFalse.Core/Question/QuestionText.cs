using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public class QuestionText
    {
        public AnswerText AnswerText; 

        public bool IsValidAnswer(UserInputText userinput)
        {
            return AnswerText.Text.Equals(userinput.Text);
        }
    }
}

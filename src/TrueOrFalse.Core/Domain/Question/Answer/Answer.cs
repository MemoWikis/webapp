using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public interface IAnswer
    {

    }

    public class Answer : IAnswer
    {
        public string Text;

        public Answer(){}

        public Answer(string answerText)
        {
            Text = answerText;
        }
    }
}

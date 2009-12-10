using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public interface IAnswer
    {

    }

    public class AnswerText : IAnswer
    {
        public string Text;
    }
}

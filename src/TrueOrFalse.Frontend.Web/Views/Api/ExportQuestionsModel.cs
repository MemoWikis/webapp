using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;

public class ExportQuestionsModel 
{

    public IEnumerable<Question> Questions { get; set; }

    public ExportQuestionsModel(IEnumerable<Question> questions)
    {
        Questions = questions;
    }
}

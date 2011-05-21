using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public static class QueryListExtensions
    {
    	public static Question GetById(this List<Question> questions, int id)
        {
            return questions.Find(question => question.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistance;

namespace TrueOrFalse.Core
{
    public class QuestionList : PersistableList<Question>
    {
		public QuestionList(){}

    	public QuestionList(IEnumerable<Question> questions)
    	{
    		AddRange(questions);
    	}

    	public Question GetById(int id)
        {
            return Find(question => question.Id == id);
        }
    }
}

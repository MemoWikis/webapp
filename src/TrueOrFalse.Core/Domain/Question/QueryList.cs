using System.Collections.Generic;

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

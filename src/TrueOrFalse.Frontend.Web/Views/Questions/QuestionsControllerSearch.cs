using System.Collections.Generic;
using TrueOrFalse.Search;

public class QuestionsControllerSearch
{
    public IList<Question> Run(QuestionSearchSpec searchSpec)
    {
        var questionIds = Sl.R<SearchQuestions>().Run(searchSpec).QuestionIds.ToArray();
        return Sl.R<QuestionRepo>().GetByIds(questionIds);
    }
}
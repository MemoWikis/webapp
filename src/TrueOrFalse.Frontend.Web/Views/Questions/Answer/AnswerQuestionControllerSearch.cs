using TrueOrFalse.Search;

public class AnswerQuestionControllerSearch
{
    public static QuestionCacheItem Run(QuestionSearchSpec searchSpec)
    {
        var questionIds = Sl.R<SearchQuestions>().Run(searchSpec).QuestionIds.ToArray();
        return EntityCache.GetQuestionById(questionIds[0]);
    }
}

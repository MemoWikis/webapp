using TrueOrFalse.Search;

public class AnswerQuestionControllerSearch
{
    public static Question Run(QuestionSearchSpec searchSpec)
    {
        var questionIds = Sl.R<SearchQuestions>().Run(searchSpec).QuestionIds.ToArray();
        return Sl.R<QuestionRepo>().GetById(questionIds[0]);
    }
}

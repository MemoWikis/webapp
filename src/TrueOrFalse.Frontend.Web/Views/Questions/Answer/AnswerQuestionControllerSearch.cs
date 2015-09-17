using TrueOrFalse.Search;

public class AnswerQuestionControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepo _questionRepo;
    private readonly SearchQuestions _searchQuestions;

    public AnswerQuestionControllerSearch(
        QuestionRepo questionRepo,
        SearchQuestions searchQuestions)
    {
        _questionRepo = questionRepo;
        _searchQuestions = searchQuestions;
    }

    public Question Run(QuestionSearchSpec searchSpec)
    {
        var questionIds = _searchQuestions.Run(searchSpec).QuestionIds.ToArray();
        return _questionRepo.GetById(questionIds[0]);
    }
}

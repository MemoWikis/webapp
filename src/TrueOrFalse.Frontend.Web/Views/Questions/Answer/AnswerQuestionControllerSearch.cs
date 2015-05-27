using TrueOrFalse.Search;

public class AnswerQuestionControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepository _questionRepository;
    private readonly SearchQuestions _searchQuestions;

    public AnswerQuestionControllerSearch(
        QuestionRepository questionRepository,
        SearchQuestions searchQuestions)
    {
        _questionRepository = questionRepository;
        _searchQuestions = searchQuestions;
    }

    public Question Run(QuestionSearchSpec searchSpec)
    {
        var questionIds = _searchQuestions.Run(searchSpec).QuestionIds.ToArray();
        return _questionRepository.GetById(questionIds[0]);
    }
}

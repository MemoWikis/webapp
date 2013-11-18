using System.Linq;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;


public class AnswerQuestionControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepository _questionRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SearchQuestions _searchQuestions;

    public AnswerQuestionControllerSearch(
        QuestionRepository questionRepository,
        SessionUiData sessionUiData,
        SearchQuestions searchQuestions)
    {
        _questionRepository = questionRepository;
        _sessionUiData = sessionUiData;
        _searchQuestions = searchQuestions;
    }

    public Question Run()
    {
        if (string.IsNullOrEmpty(_sessionUiData.SearchSpecQuestionAll.Filter.SearchTearm))
            return _questionRepository.GetBy(_sessionUiData.SearchSpecQuestionAll).Single();

        return SearchFromSOLR();
    }

    public Question SearchFromSOLR()
    {
        var questionIds = _searchQuestions.Run(
            _sessionUiData.SearchSpecQuestionAll.Filter.SearchTearm, _sessionUiData.SearchSpecQuestionAll).QuestionIds;

        return _questionRepository.GetById(questionIds[0]);
    }

}

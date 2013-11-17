using System.Collections.Generic;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

public class QuestionsControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepository _questionRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SearchQuestions _searchQuestions;

    public QuestionsControllerSearch(
        QuestionRepository questionRepository, 
        SessionUiData sessionUiData, 
        SearchQuestions searchQuestions)
    {
        _questionRepository = questionRepository;
        _sessionUiData = sessionUiData;
        _searchQuestions = searchQuestions;
    }

    public IList<Question> Run(QuestionsModel model)
    {
        _sessionUiData.SearchSpecQuestionAll.SetFilterByMe(model.FilterByMe);
        _sessionUiData.SearchSpecQuestionAll.SetFilterByAll(model.FilterByAll);
        _sessionUiData.SearchSpecQuestionAll.AddFilterByUser(model.AddFilterUser);
        _sessionUiData.SearchSpecQuestionAll.DelFilterByUser(model.DelFilterUser);

        if (!_sessionUiData.SearchSpecQuestionAll.OrderBy.IsSet())
            _sessionUiData.SearchSpecQuestionAll.OrderBy.OrderByPersonalRelevance.Desc();

        var solrResult = _searchQuestions.Run(
            _sessionUiData.SearchSpecQuestionAll.SearchTearm,
            _sessionUiData.SearchSpecQuestionAll);
            
        return _questionRepository.GetByIds(
            solrResult.QuestionIds.ToArray());
    }
}
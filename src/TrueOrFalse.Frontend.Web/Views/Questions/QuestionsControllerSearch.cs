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

    public IList<Question> Run(QuestionsModel model, QuestionSearchSpec searchSpec)
    {
        //if (!_sessionUiData.SearchSpecQuestionAll.OrderBy.IsSet())
        //    _sessionUiData.SearchSpecQuestionAll.OrderBy.OrderByPersonalRelevance.Desc();

        var solrResult = _searchQuestions.Run(
            searchSpec.Filter.SearchTearm,
            searchSpec,
            searchSpec.Filter.CreatorId,
            searchSpec.Filter.ValuatorId
        );
            
        return _questionRepository.GetByIds(
            solrResult.QuestionIds.ToArray());
    }
}
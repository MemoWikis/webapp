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

    public IList<Question> Run()
    {
        if (string.IsNullOrEmpty(_sessionUiData.QuestionSearchSpec.SearchTearm))
            return SearchFromSqlServer();
        
        return SearchFromSOLR();
    }

    private IList<Question> SearchFromSOLR()
    {
        var solrResult = _searchQuestions.Run(
            _sessionUiData.QuestionSearchSpec.SearchTearm, 
            _sessionUiData.QuestionSearchSpec);

        return _questionRepository.GetByIds(
            solrResult.QuestionIds.ToArray());
    }

    private IList<Question> SearchFromSqlServer()
    {
        var session = ServiceLocator.Resolve<ISession>();
        session.CreateCriteria<Category>();

        return _questionRepository.GetBy(
            _sessionUiData.QuestionSearchSpec,
            c => c.SetFetchMode("Categories", FetchMode.Eager
        ));
    }
}
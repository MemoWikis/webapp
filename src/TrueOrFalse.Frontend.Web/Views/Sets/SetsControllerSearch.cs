using System.Collections.Generic;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

public class SetsControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepository _questionRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SearchQuestions _searchQuestions;

    public SetsControllerSearch(
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
        if (string.IsNullOrEmpty(_sessionUiData.SearchSpecQuestion.SearchTearm))
            return SearchFromSqlServer();
        
        return SearchFromSOLR();
    }

    private IList<Question> SearchFromSOLR()
    {
        var solrResult = _searchQuestions.Run(
            _sessionUiData.SearchSpecQuestion.SearchTearm, 
            _sessionUiData.SearchSpecQuestion);

        return _questionRepository.GetByIds(
            solrResult.QuestionIds.ToArray());
    }

    private IList<Question> SearchFromSqlServer()
    {
        var session = ServiceLocator.Resolve<ISession>();
        session.CreateCriteria<Category>();

        return _questionRepository.GetBy(
            _sessionUiData.SearchSpecQuestion,
            c => c.SetFetchMode("Categories", FetchMode.Eager
        ));
    }
}
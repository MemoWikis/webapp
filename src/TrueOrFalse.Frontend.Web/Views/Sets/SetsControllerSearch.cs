using System.Collections.Generic;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

public class SetsControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly SetRepository _setRepo;
    private readonly SessionUiData _sessionUiData;
    private readonly SearchSets _searchQuestions;

    public SetsControllerSearch(
        SetRepository setRepo, 
        SessionUiData sessionUiData, 
        SearchSets searchQuestions)
    {
        _setRepo = setRepo;
        _sessionUiData = sessionUiData;
        _searchQuestions = searchQuestions;
    }

    public IList<Set> Run()
    {
        if (string.IsNullOrEmpty(_sessionUiData.SearchSpecQuestion.SearchTearm))
            return SearchFromSqlServer();
        
        return SearchFromSOLR();
    }

    private IList<Set> SearchFromSOLR()
    {
        var solrResult = _searchQuestions.Run(
            _sessionUiData.SearchSpecQuestion.SearchTearm, 
            _sessionUiData.SearchSpecQuestion);

        return _setRepo.GetByIds(solrResult.SetIds.ToArray());
    }

    private IList<Set> SearchFromSqlServer()
    {
        var session = ServiceLocator.Resolve<ISession>();
        session.CreateCriteria<Category>();

        return _setRepo.GetBy(_sessionUiData.SearchSpecQuestion);
    }
}
using System.Collections.Generic;
using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Search;
using TrueOrFalse.Web.Context;

public class QuestionsControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepository _questionRepository;
    private readonly SearchQuestions _searchQuestions;

    public QuestionsControllerSearch(
        QuestionRepository questionRepository, 
        SearchQuestions searchQuestions)
    {
        _questionRepository = questionRepository;
        _searchQuestions = searchQuestions;
    }

    public IList<Question> Run(QuestionsModel model, QuestionSearchSpec searchSpec)
    {
        var orderBy = SearchQuestionsOrderBy.None;
        if (searchSpec.OrderBy.OrderByQuality.IsCurrent()) orderBy = SearchQuestionsOrderBy.Quality;
        else if (searchSpec.OrderBy.OrderByViews.IsCurrent()) orderBy = SearchQuestionsOrderBy.Views;
        else if (searchSpec.OrderBy.OrderByCreationDate.IsCurrent()) orderBy = SearchQuestionsOrderBy.DateCreated;
        else if (searchSpec.OrderBy.OrderByPersonalRelevance.IsCurrent()) orderBy = SearchQuestionsOrderBy.Valuation;

        var solrResult = _searchQuestions.Run(
            searchSpec.Filter.SearchTerm,
            searchSpec,
            searchSpec.Filter.CreatorId,
            searchSpec.Filter.ValuatorId,
            orderBy: orderBy
        );
            
        return _questionRepository.GetByIds(
            solrResult.QuestionIds.ToArray());
    }
}
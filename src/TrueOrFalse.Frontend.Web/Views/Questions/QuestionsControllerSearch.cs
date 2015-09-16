using System.Collections.Generic;
using TrueOrFalse.Search;

public class QuestionsControllerSearch : IRegisterAsInstancePerLifetime
{
    private readonly QuestionRepo _questionRepo;
    private readonly SearchQuestions _searchQuestions;

    public QuestionsControllerSearch(
        QuestionRepo questionRepo, 
        SearchQuestions searchQuestions)
    {
        _questionRepo = questionRepo;
        _searchQuestions = searchQuestions;
    }

    public IList<Question> Run(QuestionSearchSpec searchSpec)
    {
        var questionIds = _searchQuestions.Run(searchSpec).QuestionIds.ToArray();
        return _questionRepo.GetByIds(questionIds);
    }
}
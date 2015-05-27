using System.Collections.Generic;
using TrueOrFalse.Search;

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

    public IList<Question> Run(QuestionSearchSpec searchSpec)
    {
        var questionIds = _searchQuestions.Run(searchSpec).QuestionIds.ToArray();
        return _questionRepository.GetByIds(questionIds);
    }
}
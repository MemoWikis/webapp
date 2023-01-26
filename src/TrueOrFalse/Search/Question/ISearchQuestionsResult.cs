using System.Collections.Generic;

namespace TrueOrFalse.Search;

public interface ISearchQuestionsResult
{
    IList<Question> GetQuestions();
    int Count { get; set; }
    List<int> QuestionIds { get; set; }
}
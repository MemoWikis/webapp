public class MeilisearchToQuestionMap
{
    public static MeilisearchQuestionMap Run(
        Question question,
        IList<QuestionValuationCacheItem> questionValuations)
    {
        return new MeilisearchQuestionMap
        {
            Id = question.Id,
            Pages = question.Pages.Select(c => c.Name).ToList(),
            PageIds = question.Pages.Select(c => c.Id).ToList(),
            CreatorId = question.Creator == null ? -1 : question.Creator.Id,
            Description = question.Description,
            Solution = question.Solution,
            SolutionType = (int)question.SolutionType,
            Text = question.Text,
        };
    }

    public static MeilisearchQuestionMap Run(
        QuestionCacheItem question,
        IList<QuestionValuationCacheItem> questionValuations)
    {
        return new MeilisearchQuestionMap
        {
            Id = question.Id,
            Pages = question.Pages.Select(c => c.Name).ToList(),
            PageIds = question.Pages.Select(c => c.Id).ToList(),
            CreatorId = question.Creator == null ? -1 : question.Creator.Id,
            Description = question.Description,
            Solution = question.Solution,
            SolutionType = (int)question.SolutionType,
            Text = question.Text,
            Language = question.Language
        };
    }
}
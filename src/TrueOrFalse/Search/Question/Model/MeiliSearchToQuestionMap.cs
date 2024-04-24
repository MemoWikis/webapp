namespace TrueOrFalse.Search
{
    public class MeiliSearchToQuestionMap
    {
        public static MeiliSearchQuestionMap Run(
            Question question,
            IList<QuestionValuationCacheItem> questionValuations)
        {
            return new MeiliSearchQuestionMap
            {
                Id = question.Id,
                Categories = question.Categories.Select(c => c.Name).ToList(),
                CategoryIds = question.Categories.Select(c => c.Id).ToList(),
                CreatorId = question.Creator == null ? -1 : question.Creator.Id,
                Description = question.Description,
                Solution = question.Solution,
                SolutionType = (int)question.SolutionType,
                Text = question.Text,
            };
        }
    }
}
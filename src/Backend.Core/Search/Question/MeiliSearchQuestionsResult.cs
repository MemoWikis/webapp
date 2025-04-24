namespace TrueOrFalse.Search
{
    public class MeiliSearchQuestionsResult : ISearchQuestionsResult
    {
        public int Count { get; set; }
        public List<int> QuestionIds { get; set; } = new();

        public IList<QuestionCacheItem> GetQuestions() =>
            EntityCache.GetQuestionsByIds(QuestionIds).ToList();
    }
}
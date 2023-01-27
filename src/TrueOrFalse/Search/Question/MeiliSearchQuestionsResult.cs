using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;

namespace TrueOrFalse.Search
{
    public class MeiliSearchQuestionsResult : ISearchQuestionsResult
    {
        public int Count { get; set;}
        public List<int> QuestionIds { get; set; } = new ();
        public IList<Question> GetQuestions() => Sl.QuestionRepo.GetByIds(QuestionIds); //todo change to EntityCacheItem
    }
}

using System.Collections.Generic;
using Seedworks.Lib.Persistence;
using SolrNet.Impl;

namespace TrueOrFalse.Search
{
    public class SolrSearchQuestionsResult :  ISearchQuestionsResult
    {
        /// <summary>In milliseconds</summary>
        public int QueryTime;

        /// <summary>Amount of items found</summary>
        public int Count { get; set; }

        public SpellCheckResults SpellChecking;

        public List<int> QuestionIds { get; set; } = new List<int>();

        public IPager Pager;

        public IList<Question> GetQuestions() => Sl.QuestionRepo.GetByIds(QuestionIds);
    }
}

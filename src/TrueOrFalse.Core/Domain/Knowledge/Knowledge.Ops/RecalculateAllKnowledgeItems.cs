using System.Linq;
using NHibernate;

namespace TrueOrFalse.Core
{
    public class RecalculateAllKnowledgeItems : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly RecalculateKnowledgeItem _recalculateKnowledgeItem;

        public RecalculateAllKnowledgeItems(ISession session, 
                                            RecalculateKnowledgeItem recalculateKnowledgeItem)
        {
            _session = session;
            _recalculateKnowledgeItem = recalculateKnowledgeItem;
        }

        public void Run()
        {
            var questionValuationRecords =
                _session.QueryOver<QuestionValuation>()
                    .Where(qv => qv.RelevancePersonal > 0)
                    .Select(
                        qv => qv.QuestionId,
                        qv => qv.UserId)
                    .List<object[]>();

            foreach (var item in questionValuationRecords)
                _recalculateKnowledgeItem.Run((int) item[0], (int) item[1]);
            
        }
    }
}

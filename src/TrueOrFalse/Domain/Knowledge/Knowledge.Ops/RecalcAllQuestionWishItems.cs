using NHibernate;

namespace TrueOrFalse
{
    public class RecalcAllQuestionWishItems : IRegisterAsInstancePerLifetime
    {
        private readonly ISession _session;
        private readonly RecalcQuestionWishItem _recalcQuestionWishItem;

        public RecalcAllQuestionWishItems(
            ISession session, 
            RecalcQuestionWishItem recalcQuestionWishItem)
        {
            _session = session;
            _recalcQuestionWishItem = recalcQuestionWishItem;
        }

        public void Run()
        {
            var questionValuationRecords =
                _session.QueryOver<QuestionValuation>()
                    .Where(qv => qv.RelevancePersonal >= 0)
                    .Select(
                        qv => qv.QuestionId,
                        qv => qv.UserId)
                    .List<object[]>();

            foreach (var item in questionValuationRecords)
                _recalcQuestionWishItem.Run((int) item[0], (int) item[1]);   
        }
    }
}

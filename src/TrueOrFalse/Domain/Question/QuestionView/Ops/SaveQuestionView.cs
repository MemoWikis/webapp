using NHibernate;
using TrueOrFalse.Search;

namespace TrueOrFalse
{
    public class SaveQuestionView : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionViewRepository _questionViewRepo;
        private readonly SearchIndexQuestion _searchIndexQuestion;
        private readonly ISession _session;

        public SaveQuestionView(
            QuestionViewRepository questionViewRepo, 
            SearchIndexQuestion searchIndexQuestion,
            ISession session)
        {
            _questionViewRepo = questionViewRepo;
            _searchIndexQuestion = searchIndexQuestion;
            _session = session;
        }

        public void Run(Question question, int userId)
        {
            _questionViewRepo.Create(new QuestionView{QuestionId = question.Id, UserId = userId});
            _session.CreateSQLQuery("UPDATE Question SET TotalViews = " + _questionViewRepo.GetViewCount(question.Id) + " WHERE Id = " + question.Id).
                ExecuteUpdate();

            _searchIndexQuestion.Update(question, commitDelayed: true);
        }
    }
}

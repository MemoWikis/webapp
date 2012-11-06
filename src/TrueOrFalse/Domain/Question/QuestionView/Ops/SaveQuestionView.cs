using NHibernate;

namespace TrueOrFalse
{
    public class SaveQuestionView : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionViewRepository _questionViewRepo;
        private readonly ISession _session;

        public SaveQuestionView(QuestionViewRepository questionViewRepo, ISession session)
        {
            _questionViewRepo = questionViewRepo;
            _session = session;
        }

        public void Run(int questionId, int userId)
        {
            _questionViewRepo.Create(new QuestionView{QuestionId = questionId, UserId = userId});
            _session.CreateSQLQuery("UPDATE Question SET TotalViews = " + _questionViewRepo.GetViewCount(questionId) + " WHERE Id = " + questionId).
                ExecuteUpdate();
        }
    }
}

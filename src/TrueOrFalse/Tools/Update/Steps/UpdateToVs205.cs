using TrueOrFalse;
using NHibernate;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;


namespace TrueOrFalse.Updates
{
    public class UpdateToVs205
    { 
        public static void Run()
        {
           var session =  Sl.Resolve<ISession>();

            var questions = Sl.QuestionRepo.GetAll();
            foreach (var question in questions)
            {
                var changes = Sl.QuestionChangeRepo.GetForQuestion(question.Id);
                var hasChanges = changes.Count > 0;

                if(hasChanges)
                    SetNameAndCreatedDate(question, changes[0].Id, session);
                else
                {
                    Sl.QuestionChangeRepo.Create(question);
                    changes = Sl.QuestionChangeRepo.GetForQuestion(question.Id);
                    SetNameAndCreatedDate(question, changes[0].Id,session);
                }
            }
        }

        public static void SetNameAndCreatedDate(Question question, int changeQuestionId, ISession session)
        {
            if (question.Creator != null)
                session
                    .CreateSQLQuery(
                        "Update questionChange Set Author_Id = :userId, DateCreated = :dateCreated Where Id = :questionId")
                    .SetParameter("userId", question.Creator.Id).SetParameter("questionId", changeQuestionId)
                    .SetParameter("dateCreated", question.DateCreated).ExecuteUpdate();
            else
                session
                    .CreateSQLQuery(
                        "Update questionChange Set Author_Id = null, DateCreated = :dateCreated Where Id = :questionId")
                    .SetParameter("questionId", changeQuestionId).SetParameter("dateCreated", question.DateCreated)
                    .ExecuteUpdate();
        }
    }
}
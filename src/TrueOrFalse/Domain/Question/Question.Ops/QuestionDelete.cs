using System.Linq;
using NHibernate;

public class QuestionDelete
{
    public static void Run(int questionId)
    {
        var questionRepo = Sl.R<QuestionRepo>();
        var question = questionRepo.GetById(questionId);

        ThrowIfNot_IsLoggedInUserOrAdmin.Run(question.Creator.Id);

        var categoriesToDelete = question.Categories.ToList();
        questionRepo.Delete(question);
        Sl.R<UpdateQuestionCountForCategory>().Run(categoriesToDelete);
        Sl.R<AnswerHistoryRepo>().DeleteFor(questionId);

        Sl.R<ISession>()
            .CreateSQLQuery("DELETE FROM categories_to_questions where Question_id = " + questionId)
            .ExecuteUpdate();
    }

    public static CanBeDeletedResult CanBeDeleted(int currentUserId, int questionId)
    {
        var howOftenInOtherPeopleWuwi = Sl.R<QuestionRepo>().HowOftenInOtherPeoplesWuwi(currentUserId, questionId);
        if (howOftenInOtherPeopleWuwi > 0)
        {
            return new CanBeDeletedResult
            {
                Yes = false,
                IfNot_Reason = 
                    "Die Frage kann nicht gelöscht werden, " +
                    "sie ist " + howOftenInOtherPeopleWuwi + "-mal Teil des Wunschwissens anderer Nutzer."
            };
        }

        return new CanBeDeletedResult{ Yes = true };
    }

    public class CanBeDeletedResult
    {
        public bool Yes;
        public string IfNot_Reason = "";
    }
}
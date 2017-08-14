using System;
using System.Linq;
using NHibernate;

public class QuestionDelete
{
    public static void Run(int questionId)
    {
        var questionRepo = Sl.R<QuestionRepo>();
        var question = questionRepo.GetById(questionId);
        ThrowIfNot_IsLoggedInUserOrAdmin.Run(question.Creator.Id);

        var canBeDeletedResult = CanBeDeleted(Sl.R<SessionUser>().UserId, questionId);
        if (!canBeDeletedResult.Yes)
        {
            throw new Exception("Question cannot be deleted: " + canBeDeletedResult.IfNot_Reason);
        }

        var categoriesToUpdate = question.Categories.ToList();
        //delete connected db-entries
        Sl.R<AnswerRepo>().DeleteFor(questionId); //not accounted for: answerfeature_to_answer
        Sl.R<QuestionViewRepository>().DeleteForQuestion(questionId);
        Sl.R<QuestionInSetRepo>().DeleteForQuestion(questionId);
        Sl.R<UserActivityRepo>().DeleteForQuestion(questionId);
        Sl.R<QuestionViewRepository>().DeleteForQuestion(questionId);
        Sl.R<QuestionValuationRepo>().DeleteForQuestion(questionId);
        Sl.R<CommentRepository>().DeleteForQuestion(questionId);
        Sl.R<ISession>()
            .CreateSQLQuery("DELETE FROM game_round where Question_id = :questionId")
            .SetParameter("questionId", questionId)
            .ExecuteUpdate();
        Sl.R<ISession>()
            .CreateSQLQuery("DELETE FROM categories_to_questions where Question_id = " + questionId)
            .ExecuteUpdate();
        Sl.R<ISession>()
            .CreateSQLQuery("DELETE FROM questionFeature_to_question where Question_id = " + questionId)
            .ExecuteUpdate(); //probably not necessary

        questionRepo.Delete(question);
        Sl.R<UpdateQuestionCountForCategory>().Run(categoriesToUpdate);

        var aggregatedCategoriesToUpdate = CategoryAggregation.GetAggregatingAncestors(categoriesToUpdate);

        foreach (var category in aggregatedCategoriesToUpdate)
        {
            category.UpdateCountQuestionsAggregated();
            Sl.CategoryRepo.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
        }
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
                    "sie ist " + howOftenInOtherPeopleWuwi + "-mal Teil des Wunschwissens anderer Nutzer. " +
                    "Bitte melde dich bei uns, wenn du meinst, die Frage sollte dennoch gelöscht werden."
            };
        }

        var howOftenInFutureDate = Sl.R<QuestionRepo>().HowOftenInFutureDate(questionId);
        if (howOftenInFutureDate > 0)
        {
            return new CanBeDeletedResult
            {
                Yes = false,
                IfNot_Reason =
                    "Die Frage kann nicht gelöscht werden, da in " +
                    howOftenInFutureDate + " zukünftigen Termin" + StringUtils.PluralSuffix(howOftenInFutureDate, "en") + " (vielleicht auch bei dir) damit gelernt wird. " +
                    "Bitte melde dich bei uns, wenn du meinst, die Frage sollte dennoch gelöscht werden."
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
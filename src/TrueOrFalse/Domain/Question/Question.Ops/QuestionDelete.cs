﻿using System;
using System.Linq;
using NHibernate;

public class QuestionDelete
{
    public static void Run(int questionId)
    {
        var questionRepo = Sl.QuestionRepo;
        var question = questionRepo.GetById(questionId);
        ThrowIfNot_IsLoggedInUserOrAdmin.Run(question.Creator.Id);

        var canBeDeletedResult = CanBeDeleted(Sl.R<SessionUser>().UserId, question);
        if (!canBeDeletedResult.Yes)
        {
            throw new Exception("Question cannot be deleted: Question is " + canBeDeletedResult.WuwiCount + "x in Wishknowledge");
        }

        EntityCache.Remove(question);

        var categoriesToUpdate = question.Categories.ToList();
        //delete connected db-entries
        Sl.R<ReferenceRepo>().DeleteForQuestion(questionId);
        Sl.R<AnswerRepo>().DeleteFor(questionId); //not accounted for: answerfeature_to_answer
        Sl.R<QuestionViewRepository>().DeleteForQuestion(questionId);
        Sl.R<UserActivityRepo>().DeleteForQuestion(questionId);
        Sl.R<QuestionViewRepository>().DeleteForQuestion(questionId);
        Sl.R<QuestionValuationRepo>().DeleteForQuestion(questionId);
        Sl.R<CommentRepository>().DeleteForQuestion(questionId);
        Sl.R<ISession>()
            .CreateSQLQuery("DELETE FROM categories_to_questions where Question_id = " + questionId)
            .ExecuteUpdate();
        Sl.R<ISession>()
            .CreateSQLQuery("DELETE FROM questionFeature_to_question where Question_id = " + questionId)
            .ExecuteUpdate(); //probably not necessary

        questionRepo.Delete(question);
        Sl.QuestionChangeRepo.AddDeleteEntry(question);

        Sl.R<UpdateQuestionCountForCategory>().Run(categoriesToUpdate);

        var aggregatedCategoriesToUpdate = CategoryAggregation.GetAggregatingAncestors(categoriesToUpdate);

        foreach (var category in aggregatedCategoriesToUpdate)
        {
            category.UpdateCountQuestionsAggregated();
            Sl.CategoryRepo.Update(category);
            KnowledgeSummaryUpdate.ScheduleForCategory(category.Id);
        }
    }

    public static CanBeDeletedResult CanBeDeleted(int currentUserId, Question question)
    {
        var questionCreator = question.Creator;
        if (PermissionCheck.CanDelete(question))
        {
            var howOftenInOtherPeopleWuwi = Sl.R<QuestionRepo>().HowOftenInOtherPeoplesWuwi(currentUserId, question.Id);
            if (howOftenInOtherPeopleWuwi > 0)
            {
                return new CanBeDeletedResult
                {
                    Yes = false,
                    WuwiCount = howOftenInOtherPeopleWuwi
                };
            }

            return new CanBeDeletedResult { Yes = true };
        }
        return new CanBeDeletedResult
        {
            Yes = false,
            HasRights = false
        };
    }

    public class CanBeDeletedResult
    {
        public bool Yes;
        public int WuwiCount;
        public bool HasRights = true;
    }
}

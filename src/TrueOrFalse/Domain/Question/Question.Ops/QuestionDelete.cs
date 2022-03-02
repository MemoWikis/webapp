using System;
using System.Diagnostics;
using System.Linq;
using NHibernate;
using TrueOrFalse.Utilities.ScheduledJobs;

public class QuestionDelete
{
    public static void Run(int questionId)
    {
        var questionRepo = Sl.QuestionRepo;
        var question = questionRepo.GetById(questionId);
        var questionCacheItem = EntityCache.GetQuestionCacheItem(questionId);
        ThrowIfNot_IsLoggedInUserOrAdmin.Run(question.Creator.Id);

        var canBeDeletedResult = CanBeDeleted(Sl.R<SessionUser>().UserId, question);
        if (!canBeDeletedResult.Yes)
        {
            throw new Exception("Question cannot be deleted: Question is " + canBeDeletedResult.WuwiCount + "x in Wishknowledge");
        }

        EntityCache.Remove(questionCacheItem);
        UserCache.RemoveQuestionForUser(Sl.SessionUser.UserId, questionId);
        JobScheduler.StartImmediately_DeleteQuestion(questionId);
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

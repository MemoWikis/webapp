using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using TrueOrFalse;

public class BadgeAwardCheckParams
{
    public User CurrentUser;
    public BadgeType BadgeType;

    public BadgeAwardCheckParams(BadgeType badgeType, User user)
    {
        BadgeType = badgeType;
        CurrentUser = user;
    }

    public T R<T>(){return Sl.R<T>();} 

    public int WishknowledgeCount()
    {
        return R<QuestionValuationRepo>().GetByUser(CurrentUser.Id).Count;
    }

    public int Wishknowledge_UserIsCreator()
    {
        return R<ISession>()
            .QueryOver<QuestionValuation>()
            .Where(q => q.Question.Creator.Id == CurrentUser.Id)
            .Select(Projections.Count("Id"))
            .FutureValue<int>()
            .Value;
    }

    public int Wishknowledge_OtherIsCreator()
    {
        return R<ISession>()
            .QueryOver<QuestionValuation>()
            .Where(q => q.Question.Creator.Id != CurrentUser.Id)
            .Select(Projections.Count("Id"))
            .FutureValue<int>()
            .Value;
    }

    public int AnswerCount()
    {
        return R<AnswerHistoryRepo>().GetByUser(CurrentUser.Id).Count;
    }

    public int MultipleChoiceQuestionsWithCategories()
    {
        var query = @"SELECT COUNT(*) FROM 
            (
                SELECT COUNT(q.Id)
                FROM question q
                LEFT JOIN categories_to_questions cq
                ON cq.Question_Id = q.Id
                WHERE q.Creator_id = 2
                AND q.SolutionType = 3 /*MultipleChoice*/
                AND cq.Question_Id is not null
                GROUP BY q.Id
            ) as t";

        return (int)R<ISession>().CreateSQLQuery(query).UniqueResult<long>();
    }

    public int SetsWithAtLeast10Questions()
    {
        return R<ISession>().QueryOver<Set>()
            .Where(q => q.QuestionsInSet.Count > 10)
            .Select(Projections.Count("Id"))
            .FutureValue<int>()
            .Value;
    }

    public bool IsBadgeAwarded(string badgeKey, BadgeAwardCheckParams awardCheckParams)
    {
        var badgeType = BadgeTypes.All().ByKey(badgeKey);
        return badgeType.AwardCheck(awardCheckParams).Success;
    }

    public int PlayedGames()
    {
        return R<ISession>().QueryOver<Game>()
            .Where(q => q.Players.Any(p => p.User.Id == CurrentUser.Id))
            .Select(Projections.Count("Id"))
            .FutureValue<int>()
            .Value;
    }

    public int Dates()
    {
        return R<ISession>().QueryOver<Date>()
            .Where(d => d.User.Id == CurrentUser.Id)
            .Select(Projections.Count("Id"))
            .FutureValue<int>()
            .Value;
    }

    public int UsersFollowed()
    {
        return -1;
    }
}
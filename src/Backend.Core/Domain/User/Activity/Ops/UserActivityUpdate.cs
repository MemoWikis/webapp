﻿using NHibernate;

class UserActivityUpdate
{
    public static void NewFollower(
        User userFollower,
        User userIsFollowed,
        UserActivityRepo userActivityRepo,
        ISession nhibernateSession,
        UserReadingRepo userReadingRepo)
    {
        var userActivities = new List<UserActivity>();
        AddCreatedQuestions(ref userActivities, userFollower, userIsFollowed, nhibernateSession);
        AddCreatedPage(ref userActivities, userFollower, userIsFollowed, nhibernateSession);
        AddFollowedUser(ref userActivities, userFollower, userIsFollowed, userReadingRepo);

        userActivityRepo.Create(userActivities);
    }

    private static void AddCreatedQuestions(
        ref List<UserActivity> userActivities,
        User userFollower,
        User userCauser,
        ISession nhibernateSession)
    {
        var amount = 10;
        var questions = nhibernateSession.QueryOver<Question>()
            .OrderBy(x => x.DateCreated).Desc
            .Where(q => q.Creator == userCauser && q.Visibility == QuestionVisibility.Public)
            .Take(amount)
            .List<Question>();

        foreach (var question in questions)
        {
            userActivities.Add(new UserActivity
            {
                UserConcerned = userFollower,
                At = question.DateCreated,
                Type = UserActivityType.CreatedQuestion,
                Question = question,
                UserCauser = question.Creator
            });
        }
    }

    private static void AddCreatedPage(
        ref List<UserActivity> userActivities,
        User userFollower,
        User userCauser,
        ISession nhibernateSession)
    {
        var amount = 10;
        var pages = nhibernateSession.QueryOver<Page>()
            .OrderBy(x => x.DateCreated).Desc
            .Where(q => q.Creator == userCauser)
            .Take(amount)
            .List<Page>();

        foreach (var page in pages)
        {
            userActivities.Add(new UserActivity
            {
                UserConcerned = userFollower,
                At = page.DateCreated,
                Type = UserActivityType.CreatedPage,
                Page = page,
                UserCauser = page.Creator
            });
        }
    }

    private static void AddFollowedUser(
        ref List<UserActivity> userActivities,
        User userFollower,
        User userIsFollowed,
        UserReadingRepo userReadingRepo)
    {
        var userIsFollowedFromDB =
            userReadingRepo.GetById(userIsFollowed
                .Id); //needs to update userIsFollowed, because otherwise followers wouldn't be visible here (no session)
        foreach (var follower in userIsFollowedFromDB.Following)
        {
            userActivities.Add(new UserActivity
            {
                UserConcerned = userFollower,
                At = follower.DateCreated,
                Type = UserActivityType.FollowedUser,
                UserIsFollowed = follower.User,
                UserCauser = userIsFollowed
            });
        }
    }
}
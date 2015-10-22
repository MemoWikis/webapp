using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

class UserActivityUpdate
{   
    public static void NewFollower(User userFollower, User userIsFollowed)
    {
        var userActivities = new List<UserActivity>();
        AddCreatedQuestions(ref userActivities, userFollower, userIsFollowed);
        AddCreatedCategory(ref userActivities, userFollower, userIsFollowed);
        AddCreatedSet(ref userActivities, userFollower, userIsFollowed);
        AddCreatedDate(ref userActivities, userFollower, userIsFollowed);
        AddCreatedGame(ref userActivities, userFollower, userIsFollowed);
        AddFollowedUser(ref userActivities, userFollower, userIsFollowed);

        Sl.R<UserActivityRepo>().Create(userActivities);
    }

    private static void AddCreatedQuestions(ref List<UserActivity> userActivities, User userFollower, User userCauser, int amount = 10)
    {
        var questions = Sl.R<ISession>().QueryOver<Question>()
            .OrderBy(x => x.DateCreated).Desc
            .Where(q => q.Creator == userCauser && q.Visibility == QuestionVisibility.All)
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
    private static void AddCreatedCategory(ref List<UserActivity> userActivities, User userFollower, User userCauser, int amount = 10)
    {
        var categories = Sl.R<ISession>().QueryOver<Category>()
            .OrderBy(x => x.DateCreated).Desc
            .Where(q => q.Creator == userCauser)
            .Take(amount)
            .List<Category>();

        foreach (var category in categories)
        {
            userActivities.Add(new UserActivity
            {
                UserConcerned = userFollower,
                At = category.DateCreated,
                Type = UserActivityType.CreatedCategory,
                Category = category,
                UserCauser = category.Creator
            });
        }
    }

    private static void AddCreatedSet(ref List<UserActivity> userActivities, User userFollower, User userCauser, int amount = 10)
    {
        var sets = Sl.R<ISession>().QueryOver<Set>()
            .OrderBy(x => x.DateCreated).Desc
            .Where(s => s.Creator == userCauser)
            .Take(amount)
            .List<Set>();

        foreach (var set in sets)
        {
            userActivities.Add(new UserActivity
            {
                UserConcerned = userFollower,
                At = set.DateCreated,
                Type = UserActivityType.CreatedSet,
                Set = set,
                UserCauser = set.Creator
            });
        }
    }

    private static void AddCreatedDate(ref List<UserActivity> userActivities, User userFollower, User userCauser, int amount = 10)
    {
        var dates = Sl.R<ISession>().QueryOver<Date>()
            .OrderBy(x => x.DateCreated).Desc
            .Where(d => d.User == userCauser)
            .Take(amount)
            .List<Date>();

        foreach (var date in dates)
        {
            userActivities.Add(new UserActivity
            {
                UserConcerned = userFollower,
                At = date.DateCreated,
                Type = UserActivityType.CreatedDate,
                Date = date,
                UserCauser = date.User
            });
        }
    }

    private static void AddCreatedGame(ref List<UserActivity> userActivities, User userFollower, User userCauser, int amount = 10)
    {
        var players = Sl.R<ISession>().QueryOver<Player>() //List of Players actually contains a list of Games, where always the same player is player and creator, just to get the IDs of the games this player created
            .OrderBy(x => x.DateCreated).Desc
            .Where(g => g.User == userCauser && g.IsCreator)
            .Take(amount)
            .List<Player>();

        foreach (var player in players)
        {
            userActivities.Add(new UserActivity
            {
                UserConcerned = userFollower,
                At = player.Game.DateCreated,
                Type = UserActivityType.CreatedGame,
                Game = player.Game,
                UserCauser = player.Game.Creator.User
            });
        }
    }

    private static void AddFollowedUser(ref List<UserActivity> userActivities, User userFollower, User userIsFollowed, int amount = 10)
    {
        //todo: Because DateCreated is not available yet, UserActivity.At is set to current date -> needs to be changed
        var userIsFollowedFromDB = Sl.R<UserRepo>().GetById(userIsFollowed.Id); //needs to update userIsFollowed, because otherwise followers wouldn't be visible here (no session)
        foreach (var follower in userIsFollowedFromDB.Following)
        {
            userActivities.Add(new UserActivity
            {
                UserConcerned = userFollower,
                At = DateTime.Now,
                Type = UserActivityType.FollowedUser,
                UserIsFollowed = follower.Follower,
                UserCauser = userIsFollowed
            });
        }
    }


}


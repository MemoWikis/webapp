using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

public class UserActivityAdd
{
    public static void CreatedQuestion(Question question)
    {
        var userCreator = Sl.R<UserRepo>().GetById(question.Creator.Id); //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        foreach (var follower in userCreator.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity {
                    UserConcerned = follower,
                    At = DateTime.Now,
                    Type = UserActivityType.CreatedQuestion,
                    Question = question,
                    UserCauser = question.Creator
                }); 
        }
    }

    public static void CreatedSet(Set set)
    {
        var userCreator = Sl.R<UserRepo>().GetById(set.Creator.Id); //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        foreach (var follower in userCreator.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity
            {
                UserConcerned = follower,
                At = DateTime.Now,
                Type = UserActivityType.CreatedSet,
                Set = set,
                UserCauser = set.Creator
            });
        }
    }

    public static void CreatedCategory(Category category)
    {
        var userCreator = Sl.R<UserRepo>().GetById(category.Creator.Id); //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        foreach (var follower in userCreator.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity
            {
                UserConcerned = follower,
                At = DateTime.Now,
                Type = UserActivityType.CreatedCategory,
                Category = category,
                UserCauser = category.Creator
            });
        }
    }

    public static void CreatedGame(Game game)
    {
        //todo: where to call this method? there is no Game.Create -> Userbutton verfolgen / überlagern; done
        //todo: check with Robert if Game should get Game.GetCreatorId or Game.Creator (of type User, not Player?)
        //game.Creator.User
        var userCreator = new User();
        foreach (var player in game.Players)
        {
            if (player.IsCreator)
            {
                userCreator = Sl.R<UserRepo>().GetById(player.Id);
            }
        }

        foreach (var follower in userCreator.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity
            {
                UserConcerned = follower,
                At = DateTime.Now,
                Type = UserActivityType.CreatedGame,
                Game = game,
                UserCauser = userCreator
            });
        }
    }

    public static void CreatedDate(Date date)
    {
        var userCreator = Sl.R<UserRepo>().GetById(date.User.Id); //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        foreach (var follower in userCreator.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity
            {
                UserConcerned = follower,
                At = DateTime.Now,
                Type = UserActivityType.CreatedDate,
                Date = date,
                UserCauser = date.User
            });
        }
    }

    public static void FollowedUser(User userFollows, User userIsFollowed)
    {
        foreach (var follower in userFollows.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity
            {
                UserConcerned = follower,
                At = DateTime.Now,
                Type = UserActivityType.FollowedUser,
                UserIsFollowed = userIsFollowed,
                UserCauser = userFollows
            });
        }
    }
        
}
using System;
using System.Collections.Generic;
using System.Linq;

public class UserActivityAdd
{
    public static void CreatedQuestion(Question question)
    {
        foreach (var follower in question.Creator.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity {
                    UserConcerned = follower,
                    At = DateTime.Now,
                    Type = UserActivityType.CreatedQuestion,
                    Question = question,
                    UserCauser = question.Creator
                }); 
            // todo teste mit mehreren followern
        }
    }

    public static void CreatedSet(Set set)
    {
        foreach (var follower in set.Creator.Followers)
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
    public static void FollowedUser(User userFollows, User userIsFollowed)
    {
        foreach (var follower in userFollows.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity
            {
                UserConcerned = follower,
                At = DateTime.Now,
                Type = UserActivityType.CreatedSet,
                UserIsFollowed = userIsFollowed,
                UserCauser = userFollows
            });
        }
    }
        
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetSampleUserActivities
{
    public static IList<UserActivity> Run(User userConcerned)
    {
        var userMemucho = Sl.R<UserRepo>().GetById(26);
        var userChristof = Sl.R<UserRepo>().GetById(33);
        var userRobert = Sl.R<UserRepo>().GetById(2);
        var userJule = Sl.R<UserRepo>().GetById(25);
        var userPiet = new User { Name = "Piet", Id = 0 };
        var userSarah = new User { Name = "Sarah", Id = 0 };
        var question1 = Sl.R<QuestionRepo>().GetById(491);

        var result = new List<UserActivity>();
        result.Add(new UserActivity
        {
            At = DateTime.Now.AddMinutes(-15),
            UserConcerned = userConcerned,
            UserCauser = userMemucho,
            Type = UserActivityType.CreatedDate,
        });

        result.Add(new UserActivity
        {
            At = DateTime.Now.AddHours(-10),
            UserConcerned = userConcerned,
            UserCauser = userMemucho,
            Type = UserActivityType.CreatedSet
        });

        result.Add(new UserActivity
        {
            At = DateTime.Now.AddMinutes(-60),
            UserConcerned = userConcerned,
            UserCauser = userRobert,
            Type = UserActivityType.GamePlayed,
        });

        result.Add(new UserActivity
        {
            At = DateTime.Now.AddDays(-2),
            UserConcerned = userConcerned,
            UserCauser = userChristof,
            Type = UserActivityType.FollowedUser,
            UserIsFollowed = userMemucho
        });

        result.Add(new UserActivity
        {
            At = DateTime.Now.AddDays(-3),
            UserConcerned = userConcerned,
            UserCauser = userJule,
            Type = UserActivityType.CreatedQuestion,
            Question = question1
        });

        return result;
    }
}

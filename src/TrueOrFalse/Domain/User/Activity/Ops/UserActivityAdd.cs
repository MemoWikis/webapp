public class UserActivityAdd
{
    public static void CreatedQuestion(Question question)
    {
        if(question.Creator == null)
            return;

        var userCreator = Sl.R<UserRepo>().GetById(question.Creator.Id); //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        foreach (var follower in userCreator.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity {
                    UserConcerned = follower.Follower,
                    At = DateTime.Now,
                    Type = UserActivityType.CreatedQuestion,
                    Question = question,
                    UserCauser = question.Creator
                }); 
        }
    }

    /// <summary>
    /// Add Category to UserActivityRepo
    /// </summary>
    /// <param name="category"></param>
    public static void CreatedCategory(Category category)
    {
        if(category.Creator == null)
            return;

        var userCreator = Sl.R<UserRepo>().GetById(category.Creator.Id); //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        foreach (var follower in userCreator.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity
            {
                UserConcerned = follower.Follower,
                At = DateTime.Now,
                Type = UserActivityType.CreatedCategory,
                Category = category,
                UserCauser = category.Creator
            });
        }
    }

    public static void FollowedUser(User userFollows, User userIsFollowed)
    {
        //var userFollowsFromDb = Sl.R<UserRepo>().GetById(userFollows.Id); //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        foreach (var follower in userFollows.Followers)
        {
            Sl.R<UserActivityRepo>().Create(new UserActivity
            {
                UserConcerned = follower.Follower,
                At = DateTime.Now,
                Type = UserActivityType.FollowedUser,
                UserIsFollowed = userIsFollowed,
                UserCauser = userFollows
            });
        }
        //not yet implemented: following the other way around (A follows B, C now follows B -> A so far does not get UserActivity [but: who would be Causer, C or B?])
        //when implementing this, sort out doubles (A follows B and C, C now follows B -> A should get only one UserActivity)
    }
        
}
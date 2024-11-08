public class UserActivityAdd
{
    public static void CreatedQuestion(
        Question question,
        UserReadingRepo userReadingRepo,
        UserActivityRepo userActivityRepo)
    {
        if (question.Creator == null)
            return;
        //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        var userCreator = userReadingRepo.GetById(question.Creator.Id);
        foreach (var follower in userCreator.Followers)
        {
            userActivityRepo.Create(new UserActivity
            {
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
    /// <param name="page"></param>
    public static void CreatedCategory(
        Page page,
        UserReadingRepo userReadingRepo,
        UserActivityRepo userActivityRepo)
    {
        if (page.Creator == null)
            return;
        //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        var userCreator = userReadingRepo.GetById(page.Creator.Id);
        foreach (var follower in userCreator.Followers)
        {
            userActivityRepo.Create(new UserActivity
            {
                UserConcerned = follower.Follower,
                At = DateTime.Now,
                Type = UserActivityType.CreatedCategory,
                Page = page,
                UserCauser = page.Creator
            });
        }
    }

    public static void FollowedUser(
        User userFollows,
        User userIsFollowed,
        UserActivityRepo userActivityRepo)
    {
        //var userFollowsFromDb = Sl.R<UserReadingRepo>().GetById(userFollows.Id); //need to reload user, because no session here, so lazy-load would prevent visibility of followers
        foreach (var follower in userFollows.Followers)
        {
            userActivityRepo.Create(new UserActivity
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
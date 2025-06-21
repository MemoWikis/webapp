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
    /// Add Page to UserActivityRepo
    /// </summary>
    /// <param name="page"></param>
    public static void CreatedPage(
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
                Type = UserActivityType.CreatedPage,
                Page = page,
                UserCauser = page.Creator
            });
        }
    }
}
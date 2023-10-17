
using System.Linq;
using NHibernate.Util;

public class PersonalTopicMigration
{
    public static void CreateOrAddPersonalTopicForUsersWithoutStartTopicId()
    {
        var userRepo = Sl.UserRepo;
        var users = userRepo.GetAll();
        var categoryRepo = Sl.CategoryRepo;
        var allCategories = categoryRepo.GetAll();

        foreach (var user in users)
        {
            if (user.StartTopicId <= 0)
            {
                Logg.r().Information("PersonalTopicMigration - Start Migration for User: {userId}", user.Id);
                var firstTopic = allCategories.FirstOrDefault(c => c.Creator == user);

                if (firstTopic != null && firstTopic.Name.Contains("Wiki"))
                {
                    user.StartTopicId = firstTopic.Id;
                    Logg.r().Information("PersonalTopicMigration - User: {userId}, TopicAdded: {topicId}", user.Id, firstTopic.Id);
                }
                else
                {
                    var newTopic = PersonalTopic.GetPersonalCategory(user);
                    categoryRepo.CreateOnlyDb(newTopic);
                    user.StartTopicId = newTopic.Id;
                    Logg.r().Information("PersonalTopicMigration - User: {userId}, TopicCreated: {topicId}", user.Id, newTopic.Id);
                }

                userRepo.UpdateOnlyDb(user);
                Logg.r().Information("PersonalTopicMigration - End Migration for User: {userId}", user.Id);
            }
        }
    }

}
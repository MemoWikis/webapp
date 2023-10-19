
using System.Linq;
using NHibernate.Util;

public class PersonalTopicMigration
{
    public static void CreateOrAddPersonalTopicForUsersWithoutStartTopicId(CategoryRepository categoryRepository, UserWritingRepo userWritingRepo, UserReadingRepo userReadingRepo)
    {
        var users = userReadingRepo.GetAll();
        var allCategories = categoryRepository.GetAll();

        foreach (var user in users)
        {
            if (user.StartTopicId <= 0)
            {
                Logg.r.Information("PersonalTopicMigration - Start Migration for User: {userId}", user.Id);
                var firstTopic = allCategories.FirstOrDefault(c => c.Creator == user);

                if (firstTopic != null && firstTopic.Name.Contains("Wiki"))
                {
                    user.StartTopicId = firstTopic.Id;
                    Logg.r.Information("PersonalTopicMigration - User: {userId}, TopicAdded: {topicId}", user.Id, firstTopic.Id);
                }
                else
                {
                    var newTopic = PersonalTopic.GetPersonalCategory(user, categoryRepository);
                    categoryRepository.CreateOnlyDb(newTopic);
                    user.StartTopicId = newTopic.Id;
                    Logg.r.Information("PersonalTopicMigration - User: {userId}, TopicCreated: {topicId}", user.Id, newTopic.Id);
                }

                userWritingRepo.UpdateOnlyDb(user);
                Logg.r.Information("PersonalTopicMigration - End Migration for User: {userId}", user.Id);
            }
        }
    }

}
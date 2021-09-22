using NHibernate;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs226
    {
        public static void Run()
        {
            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `user`
                    ADD COLUMN `StartTopicId` Int DEFAULT 0"
                ).ExecuteUpdate();

            Sl.Resolve<ISession>()
                .CreateSQLQuery(
                    @"ALTER TABLE `Category`
                    ADD COLUMN `IsUserStartTopic` BIT DEFAULT 0"
                ).ExecuteUpdate();

            var users =   Sl.UserRepo.GetAll();
          
          foreach (var user in users)
          {
            var category = PersonalTopic.GetPersonalCategory(user); 
            Sl.CategoryRepo.CreateOnlyDb(category);
            user.StartTopicId = category.Id; 
          }
        }
    }
}
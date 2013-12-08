using NHibernate;
using TrueOrFalse;
using TrueOrFalse.Infrastructure.Persistence;

namespace TrueOrFalse.Updates
{
    public class UpdateToVs030
    {
        public static void Run()
        {
            var userRepo = Sl.Resolve<UserRepository>();
            foreach (var user in userRepo.GetAll())
            {
                user.EmailAddress = user.EmailAddress.Trim();
                user.Name = user.Name.Trim();
                userRepo.Update(user);
            }

        }
    }
}

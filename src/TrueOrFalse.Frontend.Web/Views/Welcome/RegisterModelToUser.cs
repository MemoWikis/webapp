using TrueOrFalse.Core;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class RegisterModelToUser : ModelBase
    {
        public static User Run(RegisterModel registerModel)
        {
            var user = new User();
            user.EmailAddress = registerModel.Email;
            

            return user;
        }
    }
}

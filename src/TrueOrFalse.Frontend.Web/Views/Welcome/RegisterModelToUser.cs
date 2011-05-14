using TrueOrFalse.Core;
using TrueOrFalse.Core.Registration;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class RegisterModelToUser : ModelBase
    {
        public static User Run(RegisterModel registerModel)
        {
            var user = new User();
            user.EmailAddress = registerModel.Email;
            user.UserName = registerModel.UserName;

            SetUserPassword.Run(registerModel.Password, user);

            return user;
        }
    }
}

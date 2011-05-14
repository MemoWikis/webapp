using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core.Registration 
{
    public class SetUserPassword
    {
        public static void Run(string password, User user)
        {
            user.Salt = Guid.NewGuid().ToString();
            user.PasswordHashedAndSalted = HashPassword.Run(password, user.Salt);
        }
    }
}

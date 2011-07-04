using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core.Infrastructure;
using TrueOrFalse.Core.Registration;

namespace TrueOrFalse.Core
{
    public class SampleData : IRegisterAsInstancePerLifetime
    {
        private readonly RegisterUser _registerUser;

        public SampleData(RegisterUser registerUser)
        {
            _registerUser = registerUser;
        }

        public void CreateUsers()
        {
            var user = new User();
            user.EmailAddress = "robert@robert-m.de";
            user.UserName = "RobertM";
            SetUserPassword.Run("fooBar", user);

            _registerUser.Run(user);
        }
    }
}

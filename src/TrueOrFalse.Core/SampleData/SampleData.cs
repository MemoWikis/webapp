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
            var robert = new User();
            robert.EmailAddress = "robert@robert-m.de";
            robert.UserName = "Robert";
            SetUserPassword.Run("fooBar", robert);
            _registerUser.Run(robert);

            var jule = new User();
            jule.EmailAddress = "jule@robert-m.de";
            jule.UserName = "Jule";
            SetUserPassword.Run("fooBar", robert);
            _registerUser.Run(jule);
        }
    }
}

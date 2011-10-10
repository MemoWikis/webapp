using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDDish.Model;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class Context_RegisteredUser : IContextDescription
    {
        public string SampleDesciption { get; set; }

        public string EmailAddress = "john@doe.com";
        
        public void Setup()
        {
            var user = new User();
            user.EmailAddress = EmailAddress;
            user.Birthday = new DateTime(1980, 08, 03);

            var userRepository = BaseTest.Resolve<UserRepository>();
            userRepository.Create(user);
        }

        public Context_RegisteredUser SetEmailAddress(string emailAddress) { EmailAddress = emailAddress; return this; }
    }
}

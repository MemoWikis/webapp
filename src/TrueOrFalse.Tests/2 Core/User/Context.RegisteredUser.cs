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

        public string Name = "John Doe";
        
        public void Setup()
        {
            var user = new User();
            user.Name = Name;
            user.Birthday = new DateTime(1980, 08, 03);

            var userRepository = BaseTest.Resolve<UserRepository>();
            userRepository.Create(user);
        }

        public Context_RegisteredUser SetUserName(string userName) { Name = userName; return this; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDDish.Model;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class ContextRegisteredUser : IRegisterAsInstancePerLifetime, IContextDescription
    {
        private readonly UserRepository _userRepository;

        public string SampleDesciption { get; set; }
        public string EmailAddress = "john@doe.com";

        public List<User> Users = new List<User>();

        public ContextRegisteredUser(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public static ContextRegisteredUser New()
        {
            return BaseTest.Resolve<ContextRegisteredUser>();
        }

        public ContextRegisteredUser Add()
        {
            var user = new User();
            user.EmailAddress = EmailAddress;
            user.Birthday = new DateTime(1980, 08, 03);
            Users.Add(user);

            return this;
        }

        public ContextRegisteredUser Persist()
        {
            foreach(var user in Users)
                _userRepository.Create(user);

            return this;
        }

        public void Setup() { Add(); }

        public ContextRegisteredUser SetEmailAddress(string emailAddress) { EmailAddress = emailAddress; return this; }
    }
}

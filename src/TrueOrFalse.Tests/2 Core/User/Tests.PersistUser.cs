using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class Tests_PersistUser : BaseTest
    {
        [Test]
        public void Should_perist_user() 
        {
            var user = new User();
            user.Name = "Vorname Nachname";
            user.Birthday = new DateTime(1980, 08, 03);

            var userRepository = Resolve<UserRepository>();
            userRepository.Create(user);

            Assert.That(userRepository.GetAll().Count, Is.EqualTo(1));
        }
    }
}

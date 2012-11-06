using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class UserNavigationModelListTest
    {
        [Test]
        public void Should_not_allow_more_than_3_elements()
        {
            var list = new UserNavigationModelList();
            for (var i = 0; i < 5; i++ )
                list.Add(new UserNavigationModel(new User { Id = i, Name = "John Doe"}));

            Assert.That(list.Count(), Is.EqualTo(3));       
        }

        [Test]
        public void Should_not_allow_the_same_user_twice()
        {
            var list = new UserNavigationModelList();
            list.Add(new UserNavigationModel(new User { Id = 1, Name = "John Doe" }));
            list.Add(new UserNavigationModel(new User { Id = 1, Name = "John Doe" }));

            Assert.That(list.Count(), Is.EqualTo(1));

            var list2 = new UserNavigationModelList();
            list2.Add(new UserNavigationModel(new User { Id = 1, Name = "John Doe 1" }));
            list2.Add(new UserNavigationModel(new User { Id = 2, Name = "John Doe 2" }));

            Assert.That(list2.Count(), Is.EqualTo(2));
            Assert.That(list2.First().Name, Is.EqualTo("John Doe 2"));

            list2.Add(new UserNavigationModel(new User { Id = 1, Name = "John Doe 1" }));
            Assert.That(list2.Count(), Is.EqualTo(2));
            Assert.That(list2.First().Name, Is.EqualTo("John Doe 1"));
        }

    }
}

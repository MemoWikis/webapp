using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

    class User_cache_tests: BaseTest
    {
        [Test]
        public void Should_return_correct_bool()
        {
            var user = ContextUser.GetUser();
            var userCacheItem = UserCache.GetItem(user.Id);

            Assert.That(userCacheItem.IsFiltered, Is.EqualTo(false));
            //userCacheItem


        }
    }


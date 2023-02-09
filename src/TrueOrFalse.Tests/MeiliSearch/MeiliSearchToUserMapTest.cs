using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TrueOrFalse.Search;

namespace TrueOrFalse.Tests.MeiliSearch;

    internal class MeiliSearchToUserMapTest
    {
        [Test(Description = "Run Test successfull")]
        public void Run_successfull()
        {
            var user = new User
            {
                Id = 12,
                Name = "Test",
                DateCreated = DateTime.MaxValue,
                ReputationPos = 2000,
                WishCountQuestions = 1000
            };
            var userMap = MeiliSearchToUserMap.Run(user); 

            Assert.IsNotNull(userMap);
            Assert.AreEqual(userMap.GetType(), typeof(MeiliSearchUserMap));
            Assert.AreEqual(userMap.Id, user.Id );
            Assert.AreEqual(userMap.Name, user.Name );
            Assert.AreEqual(userMap.DateCreated, user.DateCreated );
            Assert.AreEqual(userMap.Rank, user.ReputationPos );
            Assert.AreEqual(userMap.WishCountQuestions, user.WishCountQuestions );
        }

    }

using NUnit.Framework;

namespace TrueOrFalse.Tests
{
    public class Spec_get_users_by_name : BaseTest
    {
        [Test]
        public void Should_retrieve_user_by_name()
        {
            ContextUser.New().
                Add("Usr1").
                Add("Usr2").
                Add("Usr3").Persist();

            Assert.That(Resolve<UserSearch>().Run("USR").Count, Is.EqualTo(3));
            Assert.That(Resolve<UserSearch>().Run("usr").Count, Is.EqualTo(3));
            Assert.That(Resolve<UserSearch>().Run("3").Count, Is.EqualTo(1));
            Assert.That(Resolve<UserSearch>().Run("sr2").Count, Is.EqualTo(1));
        }

        [Test]
        public void Should_retrieve_limited_result_set()
        {
            var userContext = ContextUser.New();
            for (var i = 0; i < 100; i++) userContext.Add("Usr" + i);
            userContext.Persist();

            Assert.That(Resolve<UserSearch>().Run("USR").Count, Is.EqualTo(20));
        }

    }
}
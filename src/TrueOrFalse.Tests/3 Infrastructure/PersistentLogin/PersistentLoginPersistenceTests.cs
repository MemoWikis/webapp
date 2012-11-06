using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse;

namespace TrueOrFalse.Tests
{
    public class PersistentLoginPersistenceTests : BaseTest
    {
        [Test]
        public void Should_CRUD_entry()
        {

            var guid = Guid.NewGuid().ToString();
            var persitentLogin = new PersistentLogin {UserId = 10, LoginGuid = guid};
            
            //Created
            Resolve<PersistentLoginRepository>().Create(persitentLogin);

            //Retrieve
            Assert.That(Resolve<PersistentLoginRepository>().Get(persitentLogin.UserId, guid), Is.Not.Null);

            //Delete
            Resolve<PersistentLoginRepository>().Delete(persitentLogin.UserId, guid);
            Assert.That(Resolve<PersistentLoginRepository>().Get(persitentLogin.UserId, guid), Is.Null);
        }

        [Test]
        public void Should_delete_all_for_user()
        {
            Resolve<PersistentLoginRepository>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });
            Resolve<PersistentLoginRepository>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });
            Resolve<PersistentLoginRepository>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });
            Resolve<PersistentLoginRepository>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });
            Resolve<PersistentLoginRepository>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });

            Assert.That(Resolve<ISession>().QueryOver<PersistentLogin>().Where(x => x.UserId == 10).List().Count, Is.EqualTo(5) );
            Resolve<PersistentLoginRepository>().DeleteAllForUser(10);
            Assert.That(Resolve<ISession>().QueryOver<PersistentLogin>().Where(x => x.UserId == 10).List().Count, Is.EqualTo(0));
        }

    }
}

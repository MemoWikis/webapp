using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NUnit.Framework;
using TrueOrFalse.Core;

namespace TrueOrFalse.Tests
{
    public class PersistentLoginPersistenceTests : BaseTest
    {
        [Test]
        public void Should_CRUD_entry(){
            var persitentLogin = new PersistentLogin {UserId = 10, LoginGuid = Guid.NewGuid().ToString()};
            
            //Created
            Resolve<PersistentLoginRepository>().Create(persitentLogin);

            //Retrieve
            Assert.That(Resolve<PersistentLoginRepository>().Get(persitentLogin.UserId, persitentLogin.LoginGuid), Is.Not.Null);

            //Delete
            Resolve<PersistentLoginRepository>().Delete(persitentLogin.UserId, persitentLogin.LoginGuid);
            Assert.That(Resolve<PersistentLoginRepository>().Get(persitentLogin.UserId, persitentLogin.LoginGuid), Is.Null);
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

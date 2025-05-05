using System;
using NHibernate;
using NUnit.Framework;

namespace TrueOrFalse.Tests;

public class PersistentLoginPersistenceTests : BaseTest
{
    [Test]
    public void Should_CRUD_entry()
    {
        var guid = Guid.NewGuid().ToString();
        var persitentLogin = new PersistentLogin {UserId = 10, LoginGuid = guid};
            
        //Created
        Resolve<PersistentLoginRepo>().Create(persitentLogin);

        //Retrieve
        Assert.That(Resolve<PersistentLoginRepo>().Get(persitentLogin.UserId, guid), Is.Not.Null);

        //Delete
        Resolve<PersistentLoginRepo>().Delete(persitentLogin.UserId, guid);
        Assert.That(Resolve<PersistentLoginRepo>().Get(persitentLogin.UserId, guid), Is.Null);
    }

    [Test]
    public void Should_delete_all_for_user()
    {
        Resolve<PersistentLoginRepo>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });
        Resolve<PersistentLoginRepo>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });
        Resolve<PersistentLoginRepo>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });
        Resolve<PersistentLoginRepo>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });
        Resolve<PersistentLoginRepo>().Create(new PersistentLogin { UserId = 10, LoginGuid = Guid.NewGuid().ToString() });

        Assert.That(Resolve<ISession>().QueryOver<PersistentLogin>().Where(x => x.UserId == 10).List().Count, Is.EqualTo(5) );
        Resolve<PersistentLoginRepo>().DeleteAllForUser(10);
        Assert.That(Resolve<ISession>().QueryOver<PersistentLogin>().Where(x => x.UserId == 10).List().Count, Is.EqualTo(0));
    }

}
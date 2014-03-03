using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.PasswordHashedAndSalted);
            Map(x => x.Salt);
            Map(x => x.EmailAddress);
            Map(x => x.Name);
            Map(x => x.IsEmailConfirmed);
            Map(x => x.IsInstallationAdmin);
            Map(x => x.AllowsSupportiveLogin);

            Map(x => x.Reputation);
            Map(x => x.ReputationPos);

            Map(x => x.WishCountQuestions);
            Map(x => x.WishCountSets);

            Map(x => x.Birthday);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);
        }
    }
}
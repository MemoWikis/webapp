using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.UserName);
            Map(x => x.PasswordHashedAndSalted);
            Map(x => x.EmailAddress);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.IsEmailConfirmed);
            Map(x => x.Birthday);
            Map(x => x.DateCreated);
            Map(x => x.DateModified);            
        }
    }
}

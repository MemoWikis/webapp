using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class User : DomainEntity
    {
        public virtual string UserName { get; set; }
        public virtual string PasswordHashedAndSalted { get; set; }
        public virtual string Salt { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual Boolean IsEmailConfirmed { get; set;  }

        public virtual DateTime? Birthday { get; set; }
    }
}

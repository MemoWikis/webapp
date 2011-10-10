using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seedworks.Lib.Persistence;

namespace TrueOrFalse.Core
{
    public class User : DomainEntity
    {
        public virtual string PasswordHashedAndSalted { get; set; }
        public virtual string Salt { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string Name { get; set; }

        public virtual Boolean IsEmailConfirmed { get; set;  }

        public virtual DateTime? Birthday { get; set; }
    }
}

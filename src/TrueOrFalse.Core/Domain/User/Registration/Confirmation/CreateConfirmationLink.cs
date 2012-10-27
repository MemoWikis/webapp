using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Core.Registration
{
    public class CreateEmailConfirmationLink : IRegisterAsInstancePerLifetime
    {
        public string Run(User user){
            return String.Format("http://richtig-oder-falsch.de/EmailConfirmation/x7b" + user.Id);
        }
    }
}

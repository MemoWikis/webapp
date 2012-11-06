using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse.Registration
{
    public class CreateEmailConfirmationLink : IRegisterAsInstancePerLifetime
    {
        public string Run(User user){
            return String.Format("http://richtig-oder-falsch.de/EmailConfirmation/x7b" + user.Id);
        }
    }
}

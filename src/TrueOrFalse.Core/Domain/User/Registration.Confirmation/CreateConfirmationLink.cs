using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core.Registration
{
    public class CreateEmailConfirmationLink
    {
        public string Run(User user){
            return String.Format("http://richtig-oder-falsch.de/EmailConfirmation/x7b" + user.Id);
        }
    }
}

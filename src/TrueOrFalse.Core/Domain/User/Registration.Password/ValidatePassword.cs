using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core.Registration
{
    public class IsValdidPassword
    {
        public bool True(string givenPasswordString, string dbPassword, string dbSalt)
        {
            return dbPassword == HashPassword.Run(givenPasswordString.Trim(), dbSalt);
        }
    }
}

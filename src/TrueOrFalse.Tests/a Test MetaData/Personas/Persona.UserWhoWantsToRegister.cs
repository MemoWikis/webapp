using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDDish.Model;

namespace TrueOrFalse.Tests
{
    public class UserWhoWantsToRegister : ICustomerDescription
    {
        public string About { get; set; }
    }
}

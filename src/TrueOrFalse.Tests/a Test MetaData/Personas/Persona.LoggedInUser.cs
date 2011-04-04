using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDDish.Model;

namespace TrueOrFalse.Tests
{
    public class LoggedInUser : ICustomerDescription
    {
        public string About { get; set; }
    }
}

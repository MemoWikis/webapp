using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Tests
{
    public class OneEmailWasSend
    {
        public static bool IsTrue()
        {
            return GetEmailsFromPickupDirectory.Run().Count() == 1;
        }
    }
}

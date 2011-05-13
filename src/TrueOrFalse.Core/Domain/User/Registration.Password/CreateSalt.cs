using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core.Registration
{
    public class CreateSalt
    {
        public static string Run()
        {
            return Guid.NewGuid().ToString();
        }

    }
}

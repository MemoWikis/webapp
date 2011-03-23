using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class ShowRightLogin
    {
        public bool Yes;
        public bool ShowRegisterBlock = true;

        public ShowRightLogin(bool yes){ Yes = yes; }
    }
}
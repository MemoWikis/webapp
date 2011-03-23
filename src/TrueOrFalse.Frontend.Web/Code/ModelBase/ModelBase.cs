using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class ModelBase
    {
        public ShowRightLogin ShowRightLogin = new ShowRightLogin(false);
        public bool ShowLeftMenu = true;
    }
}
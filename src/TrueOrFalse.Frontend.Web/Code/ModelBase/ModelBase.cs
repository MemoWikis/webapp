﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class ModelBase
    {
        public readonly ShowRightLogin ShowRightLogin = new ShowRightLogin(false);
        public bool ShowLeftMenu = true;
    }
}
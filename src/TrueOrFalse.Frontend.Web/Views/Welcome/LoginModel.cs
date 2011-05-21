using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse.Frontend.Web.Models
{
    public class LoginModel : ModelBase
    {
        public bool IsError;
        public string ErrorMessage;

        public string UserName;
        public string Password;

        public LoginModel()
        {
            ShowLeftMenu = false;
        }
    }
}
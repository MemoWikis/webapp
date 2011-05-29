using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse.View.Web.Views.Shared
{
    public class ErrorMessageModel
    {
        public string Message;

        public ErrorMessageModel(){}

        public ErrorMessageModel(string message)
        {
            Message = message;
        }
    }
}
using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;

namespace TrueOrFalse.Web
{
    public class SuccessMessage : Message
    {
        public SuccessMessage(string text) : base(MessageType.IsSuccess, text){}
    }

    public class ErrorMessage : Message
    {
        public ErrorMessage(string text) : base(MessageType.IsError, text){}

        public ErrorMessage(ModelStateDictionary modelState) :
            this(modelState.SelectMany(x => x.Value.Errors).ToList().Select(x => x.ErrorMessage).Aggregate((a,b) => a + "<br>" + b))
        {
        }
    }

    public class Message
    {
        public readonly string Text;
        public readonly MessageType Type;

        public string CssClass
        {
            get
            {
                if (Type == MessageType.IsError)
                    return "alert alert-danger";

                return "alert alert-success";
            }
        }

        public Message(){}

        public Message(MessageType messageType, string message)
        {
            Text = message;
            Type = messageType;
        }
    }
}
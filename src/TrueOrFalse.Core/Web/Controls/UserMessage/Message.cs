using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueOrFalse.Core.Web
{
    public class SuccessMessage : Message
    {
        public SuccessMessage(string text) : base(MessageType.IsSuccess, text){}
    }

    public class ErrorMessage : Message
    {
        public ErrorMessage(string text) : base(MessageType.IsError, text){}
    }

    public class Message
    {
        public readonly string Text;
        public readonly MessageType Type;

        public string Style
        {
            get
            {
                if (Type == MessageType.IsError)
                    return "border:1px solid red; background-color:#FFDBDD;";

                return "background-color:rgb(217, 255, 189); border:1px solid rgb(0, 194, 0);";
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
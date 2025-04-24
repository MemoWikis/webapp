using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TrueOrFalse.Web
{
    [Serializable]
    public class SuccessMessage : UIMessage
    {
        public SuccessMessage(string text) : base(MessageType.IsSuccess, text){}
    }

    [Serializable]
    public class ErrorMessage : UIMessage
    {
        public ErrorMessage(string text) : base(MessageType.IsError, text){}

        public ErrorMessage(ModelStateDictionary modelState) :
            this(modelState.SelectMany(x => x.Value.Errors).ToList().Select(x => x.ErrorMessage).Aggregate((a,b) => a + "<br>" + b))
        {
        }
    }

    [Serializable]
    public class UIMessage
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

        public UIMessage(){}

        public UIMessage(MessageType messageType, string message)
        {
            Text = message;
            Type = messageType;
        }
    }
}
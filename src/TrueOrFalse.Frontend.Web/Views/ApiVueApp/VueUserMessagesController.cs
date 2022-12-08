using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

namespace VueApp;

public class VueUserMessagesController : BaseController
{
    [HttpGet]
    public JsonResult Get()
    {
        if (SessionUser.IsLoggedIn)
        {
            var messages = Resolve<MessageRepo>()
            .GetForUser(SessionUser.UserId, false)
            .Select(m => new MessageModelRow(m))
            .ToList();

            var readMessagesCount = Resolve<MessageRepo>().GetNumberOfReadMessages(SessionUser.UserId);
            return Json(new
            {
                Messages = messages,
                ReadMessagesCount = readMessagesCount   
            }, JsonRequestBehavior.AllowGet);
        }

        return Json(new
        {
            NotLoggedIn = true,
        }, JsonRequestBehavior.AllowGet);


    }

    public class MessageModelRow
    {
        public int MessageId;
        public bool IsRead;
        public string Subject;
        public string Body;
        public string When;
        public string WhenDatetime;

        public MessageModelRow(Message message)
        {
            MessageId = message.Id;
            IsRead = message.IsRead;
            Subject = message.Subject;
            Body = message.Body;
            When = DateTimeUtils.TimeElapsedAsText(message.DateCreated);
            WhenDatetime = message.DateCreated.ToString("", new CultureInfo("de-DE"));
        }
    }
}

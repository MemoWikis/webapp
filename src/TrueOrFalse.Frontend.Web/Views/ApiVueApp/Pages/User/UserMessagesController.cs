using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using NHibernate;


namespace VueApp;

public class UserMessagesController : BaseController
{
    [HttpGet]
    public JsonResult Get()
    {
        if (SessionUserLegacy.IsLoggedIn)
        {
            var messages = Resolve<MessageRepo>()
            .GetForUser(SessionUserLegacy.UserId, false)
            .Select(m => new 
            {
                id = m.Id,
                read = m.IsRead,
                subject = m.Subject,
                body = m.Body,
                timeElapsed = DateTimeUtils.TimeElapsedAsText(m.DateCreated),
                date = m.DateCreated.ToString("", new CultureInfo("de-DE"))
            })
            .ToArray();

            var readMessagesCount = Resolve<MessageRepo>().GetNumberOfReadMessages(SessionUserLegacy.UserId);
            return Json(new
            {
                messages = messages,
                readCount = readMessagesCount   
            }, JsonRequestBehavior.AllowGet);
        }

        return Json(new
        {
            notLoggedIn = true,
        }, JsonRequestBehavior.AllowGet);
    }
}

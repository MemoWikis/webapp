using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class UserMessagesController : BaseController
{
    private readonly MessageRepo _messageRepo;

    public UserMessagesController(SessionUser sessionUser,
        MessageRepo messageRepo) :base(sessionUser)
    {
        _messageRepo = messageRepo;
    }
    [HttpGet]
    public JsonResult Get()
    {
        if (_sessionUser.IsLoggedIn)
        {
            var messages = _messageRepo
            .GetForUser(_sessionUser.UserId, false)
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

            var readMessagesCount = _messageRepo.GetNumberOfReadMessages(_sessionUser.UserId);
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

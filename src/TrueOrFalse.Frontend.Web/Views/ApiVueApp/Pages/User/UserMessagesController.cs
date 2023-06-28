﻿using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace VueApp;

public class UserMessagesController : BaseController
{
    public UserMessagesController(SessionUser sessionUser) :base(sessionUser)
    {
        
    }
    [HttpGet]
    public JsonResult Get()
    {
        if (_sessionUser.IsLoggedIn)
        {
            var messages = Resolve<MessageRepo>()
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

            var readMessagesCount = Resolve<MessageRepo>().GetNumberOfReadMessages(_sessionUser.UserId);
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

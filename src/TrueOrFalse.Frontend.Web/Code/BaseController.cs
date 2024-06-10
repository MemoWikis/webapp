﻿using System;
using Microsoft.AspNetCore.Mvc;
using System.Text;

public class BaseController : Controller
{
    protected SessionUser _sessionUser;

    public BaseController(SessionUser sessionUser)
    {
        _sessionUser = sessionUser;
    }
    public int UserId => _sessionUser.UserId;

    protected JsonResult Json(object data, string contentType, Encoding contentEncoding)
    {
        return new JsonResult(
            new {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                MaxJsonLength = Int32.MaxValue
            });

    }
}
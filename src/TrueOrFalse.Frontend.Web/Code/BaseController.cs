﻿using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

[AccessBeta]
public class BaseController : Controller
{
    protected SessionUser _sessionUser{ get { return Resolve<SessionUser>(); } }
    protected SessionUiData _sessionUiData { get { return Resolve<SessionUiData>(); } }
    public int UserId { get { return _sessionUser.UserId; } }

    protected T Resolve<T>()
    {
        return ServiceLocator.Resolve<T>();
    }

    protected T R<T>()
    {
        return ServiceLocator.Resolve<T>();
    }
}
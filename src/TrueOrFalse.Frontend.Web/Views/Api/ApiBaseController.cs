using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class ApiBaseController : Controller
{
    protected T Resolve<T>()
    {
        return ServiceLocator.Resolve<T>();
    }

    protected T R<T>()
    {
        return ServiceLocator.Resolve<T>();
    }
}
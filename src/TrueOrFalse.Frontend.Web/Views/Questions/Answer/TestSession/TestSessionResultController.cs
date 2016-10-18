using System;
using System.Web.Mvc;

public class TestSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/TestSession/TestSessionResult.aspx";

    public ActionResult TestSessionResult()
    {
        //check for possible errors?

        return View(_viewLocation, new TestSessionResultModel());
    }

}

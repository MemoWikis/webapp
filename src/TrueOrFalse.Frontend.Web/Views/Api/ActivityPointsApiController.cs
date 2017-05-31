using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Search;

public class ActivityPointsApiController : BaseController
{
    public int AddActivityPoints(int points)
    {
        //add Points to Session
        return (int) Session["TotalActivityPoints"];
    }
}
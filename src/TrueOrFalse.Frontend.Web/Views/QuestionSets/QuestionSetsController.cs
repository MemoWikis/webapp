using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public class QuestionSetsController : BaseController
{
    public ActionResult QuestionSets()
    {
        return View(new QuestionSetsModel());
    }       
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class LearningSessionResultController : BaseController
{
    private const string _viewLocation = "~/Views/Questions/Answer/LearningSession/LearningSessionResult.aspx";

    public ActionResult LearningSessionResult(int learningSessionId)
    {

        var learningSession = Sl.Resolve<LearningSessionRepo>().GetById(learningSessionId);

        return View(_viewLocation, new LearningSessionResultModel(learningSession));
    }

}

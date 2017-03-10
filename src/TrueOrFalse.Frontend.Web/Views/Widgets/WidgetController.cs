using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

[SetMenu(MenuEntry.None)]
public class WidgetController : BaseController
{
    public ActionResult Question(int questionId)
    {
        var answerQuestionModel = new AnswerQuestionModel(
            Guid.NewGuid(),
            R<QuestionRepo>().GetById(questionId),
            new QuestionSearchSpec());

        answerQuestionModel.DisableCommentLink = true;

        return View(
            "~/Views/Widgets/WidgetQuestion.aspx",
            new WidgetQuestionModel(answerQuestionModel));
    }

    public ActionResult Set(int setId)
    {
        var set = Sl.SetRepo.GetById(setId);
        var testSession = new TestSession(set);

        Sl.SessionUser.AddTestSession(testSession);

        //testSession.Id

        //var sessionCount = _sessionUser.TestSessions.Count(s => s.Id == testSessionId);

        //if (sessionCount == 0)
        //{
        //    //Logg.r().Error("SessionCount 0");
        //    //return View(_viewLocation, AnswerQuestionModel.CreateExpiredTestSession());
        //    throw new Exception("SessionCount is 0. Shoult be 1");
        //}

        //if (sessionCount > 1)
        //    throw new Exception($"SessionCount is {_sessionUser.TestSessions.Count(s => s.Id == testSessionId)}. Should be not more then more than 1.");

        //var testSession = _sessionUser.TestSessions.Find(s => s.Id == testSessionId);

        if (testSession.CurrentStep > testSession.NumberOfSteps)
            return RedirectToAction(
                Links.TestSessionResultAction, 
                Links.TestSessionResultController, 
                new { name = testSession.UriName, testSessionId = testSession.Id });

        var question = Sl.R<QuestionRepo>().GetById(testSession.Steps.ElementAt(testSession.CurrentStep - 1).QuestionId);
        var questionViewGuid = Guid.NewGuid();

        Sl.SaveQuestionView.Run(questionViewGuid, question, _sessionUser.User);

        var answerModel = new AnswerQuestionModel(testSession, questionViewGuid, question);

        return View(
            "~/Views/Widgets/WidgetSet.aspx",
            new WidgetSetModel(answerModel));
    }
}
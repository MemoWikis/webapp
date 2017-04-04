using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using TrueOrFalse;

public class TestSessionController : BaseController
{

    [HttpPost]
    public void RegisterQuestionAnswered(int testSessionId, int questionId, Guid questionViewGuid, bool answeredQuestion)
    {
        _sessionUser.AnsweredQuestionIds.Add(questionId);
        var currentStep = _sessionUser.TestSessions.Find(s => s.Id == testSessionId).CurrentStep - 1;
        //_sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps
        _sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps.ElementAt(currentStep).QuestionViewGuid = questionViewGuid;
        if (answeredQuestion)
        {
            var answers = Sl.R<AnswerRepo>().GetByQuestionViewGuid(questionViewGuid).Where(a => !a.IsView()).ToList();

            if (answers.FirstOrDefault().Question.SolutionType == SolutionType.MatchList)
            {
                var answerObject = QuestionSolutionMatchList.deserializeMatchListAnswer(answers.FirstOrDefault().AnswerText);
                string newAnswer = "</br><ul>";
                foreach (var pair in answerObject.Pairs)
                {
                    newAnswer += "<li>" + pair.ElementLeft.Text + " - " + pair.ElementRight.Text + "</li>";
                }
                newAnswer += "</ul>";
            }

            if (answers.FirstOrDefault().Question.SolutionType == SolutionType.MultipleChoice)
            {
                //TODO:Julian in Methode auslagern
                if (answers.FirstOrDefault().AnswerText != "")
                {
                    var builder = new StringBuilder(answers.FirstOrDefault().AnswerText);
                    answers.FirstOrDefault().AnswerText = "</br> <ul> <li>" +
                                                          builder.Replace("%seperate&xyz%", "</li><li>").ToString() +
                                                          "</li> </ul>";
                }
                else
                {
                    answers.FirstOrDefault().AnswerText = "(keine Auswahl)";
                }
            }

            if (answers.Count > 1)
                throw new Exception("Cannot handle multiple answers to one TestSessionStep.");
            _sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps.ElementAt(currentStep).AnswerText = answers.FirstOrDefault().AnswerText;
            _sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps.ElementAt(currentStep).AnswerState =
                answers.FirstOrDefault().AnsweredCorrectly() ? TestSessionStepAnswerState.AnsweredCorrect : TestSessionStepAnswerState.AnsweredWrong;
        }
        else
        {
            _sessionUser.TestSessions.Find(s => s.Id == testSessionId).Steps.ElementAt(currentStep).AnswerState = TestSessionStepAnswerState.OnlyViewedSolution;
        }
        _sessionUser.TestSessions.Find(s => s.Id == testSessionId).CurrentStep++;
    }

}

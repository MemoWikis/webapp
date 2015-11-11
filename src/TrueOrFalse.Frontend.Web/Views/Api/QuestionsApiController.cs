using System;
using System.Web.Mvc;

namespace Api
{
    public class QuestionsApiController : BaseController

    {
        private readonly QuestionRepo _questionRepo;

        public QuestionsApiController(
            QuestionRepo questionRepo)
        {
            _questionRepo = questionRepo;
        }

        [HttpPost]
        public void Pin(string questionId)
        {
            QuestionInKnowledge.Pin(Convert.ToInt32(questionId), _sessionUser.User);
        }

        [HttpPost]
        public void Unpin(string questionId)
        {
            QuestionInKnowledge.Unpin(Convert.ToInt32(questionId), _sessionUser.User);
        }
    }
}

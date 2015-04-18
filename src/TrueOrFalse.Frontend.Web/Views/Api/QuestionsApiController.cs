using System;
using System.Web.Mvc;

namespace Api
{
    public class QuestionsApiController : BaseController

    {
        private readonly QuestionRepository _questionRepo;

        public QuestionsApiController(
            QuestionRepository questionRepo)
        {
            _questionRepo = questionRepo;
        }

        [HttpPost]
        public void Pin(string questionId)
        {
            Resolve<UpdateQuestionTotals>()
                .UpdateRelevancePersonal(Convert.ToInt32(questionId), _sessionUser.User);
        }

        [HttpPost]
        public void Unpin(string questionId)
        {
            Resolve<UpdateQuestionTotals>()
                .UpdateRelevancePersonal(Convert.ToInt32(questionId), _sessionUser.User, -1);
        }
    }
}

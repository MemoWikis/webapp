using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

namespace Api
{
    public class QuestionsApiController : BaseController
    {
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

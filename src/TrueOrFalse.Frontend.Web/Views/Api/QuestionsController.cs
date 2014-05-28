using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

namespace Api
{
    public class QuestionsController : BaseController
    {
        [HttpPost]
        public void Pin(int questionId)
        {
            Resolve<UpdateQuestionTotals>()
                .UpdateRelevancePersonal(questionId, _sessionUser.User);
        }

        [HttpPost]
        public void Unpin(int questionId)
        {
            Resolve<UpdateQuestionTotals>()
                .UpdateRelevancePersonal(questionId, _sessionUser.User, -1);
        }
    }
}

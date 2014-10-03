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
        public class ReferenceJsonResult
        {
            public int referenceId { get; set; }
            public string referenceType { get; set; }
            public int categoryId { get; set; }
        }

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

        public JsonResult GetReferencesAsJson(int id)
        {
            var references = _questionRepo.GetById(id).References;
            
            var result = references.Select(r =>
                new ReferenceJsonResult
                {
                    referenceId = r.Id,
                    categoryId = r.Category == null ? -1 : r.Category.Id,
                    referenceType = r.ReferenceType.GetName()
                }
            ).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

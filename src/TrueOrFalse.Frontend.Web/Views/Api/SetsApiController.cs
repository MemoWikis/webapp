using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class SetsApiController : ApiBaseController
{
    public JsonResult ForQuestion(int questionId)
    {
        var result = R<QuestionInSetRepo>().Query
            .Where(x => x.Question.Id == questionId)
            .List();

        var distinctSets = result
            .Where(x => x.Set != null)
            .GroupBy(x => x.Set.Id)
            .Select(x => x.First())
            .Select(x => x.Set);

        return Json(from s in distinctSets select 
                    new {
                        Id = s.Id,
                        Name = s.Name,
                    });
    }    
}
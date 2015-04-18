using System;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class SetsApiController : BaseController
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
                        Url = Links.SetDetail(s.Name, s.Id)
                    });
    }

    [HttpPost]
    public void Pin(string setId)
    {
        Resolve<UpdateSetsTotals>()
            .UpdateRelevancePersonal(Convert.ToInt32(setId), _sessionUser.User);
    }

    [HttpPost]
    public void Unpin(string setId)
    {
        Resolve<UpdateSetsTotals>()
            .UpdateRelevancePersonal(Convert.ToInt32(setId), _sessionUser.User, -1);
    }
}
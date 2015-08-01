using System;
using System.Linq;
using System.Web.Mvc;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Search;

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

    public JsonResult ByName(string term)
    {
        var setIds = R<SearchSets>().Run(term, new Pager{PageSize = 1}, searchOnlyWithStartingWith: true).SetIds;
        var sets = R<SetRepo>().GetByIds(setIds);

        var items = sets.Select(set =>
                new SetJsonResult 
                {
                    Id = set.Id,
                    Name = set.Name,
                    NumberOfQuestions = set.QuestionsInSet.Count,
                    ImageUrl = SetImageSettings.Create(set.Id).GetUrl_50px_square().Url,
                }
            ).ToList();

        return Json(new{
            Items = items
        }, JsonRequestBehavior.AllowGet);
    }
}

public class SetJsonResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfQuestions { get; set; }
    public string ImageUrl { get; set; }
}
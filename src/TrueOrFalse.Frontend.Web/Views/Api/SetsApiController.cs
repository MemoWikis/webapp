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
        if (_sessionUser.User == null)
            return;

        var setIdInt = Convert.ToInt32(setId);

        SetInKnowledge.Pin(setIdInt, _sessionUser.User);
    
    }

    [HttpPost]
    public void Unpin(string setId)
    {
        if (_sessionUser.User == null)
            return;

        SetInKnowledge.Unpin(Convert.ToInt32(setId), _sessionUser.User);
    }

    [HttpPost]
    public void UnpinQuestionsInSet(string setId)
    {
        if (_sessionUser.User == null)
            return;

        SetInKnowledge.UnpinQuestionsInSet(Convert.ToInt32(setId), _sessionUser.User);
    }

    //public JsonResult ByName(string term)
    //{
    //    var setIds = R<SearchSets>().Run(term, new Pager{PageSize = 5}, searchOnlyWithStartingWith: true).SetIds;
    //    var sets = R<SetRepo>().GetByIds(setIds);

    //    var items = sets.Select(set =>
    //            new SetJsonResult 
    //            {
    //                Id = set.Id,
    //                Name = set.Name,
    //                NumberOfQuestions = set.QuestionsInSet.Count,
    //                ImageUrl = new SetImageSettings(set.Id).GetUrl_50px_square().Url,
    //            }
    //        ).ToList();

    //    return Json(new{
    //        Items = items
    //    }, JsonRequestBehavior.AllowGet);
    //}
}

public class SetJsonResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfQuestions { get; set; }
    public string ImageUrl { get; set; }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Web.Mvc;
using Newtonsoft.Json;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class EditSetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Edit/EditSet.aspx";

    [SetMenu(MenuEntry.QuestionSet)]
    [SetThemeMenu]
    public ActionResult Create()
    {
        var model = new EditSetModel();
        model.SetToCreateModel();
        return View(_viewLocation, model);
    }

    [HttpPost]
    [SetMenu(MenuEntry.QuestionSet)]
    [SetThemeMenu]
    public ActionResult Create(EditSetModel model)
    {
        if (!ModelState.IsValid){
            model.Message = new ErrorMessage(ModelState);
            return View(_viewLocation, model);
        }

        var set = model.ToQuestionSet();
        set.Creator = _sessionUser.User;
        Sl.SetRepo.Create(set);

        StoreImage(set.Id);

        model = new EditSetModel(set);
        model.SetToUpdateModel();

        TempData["createSetMsg"] = new SuccessMessage(
            $"Das Lernset <i>'{set.Name.TruncateAtWord(30)}'</i> wurde erstellt. Du kannst es nun weiter bearbeiten.");

        return Redirect(Links.QuestionSetEdit(set.Name, set.Id));
    }

    [SetMenu(MenuEntry.None)]
    [SetThemeMenu(isQuestionSetPage: true)]
    public ViewResult Edit(int id)
    {
        var set = Sl.SetRepo.GetById(id);
        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set, HistoryItemType.Edit));
        var model = new EditSetModel(set);
        model.SetToUpdateModel();

        if (!IsAllowedTo.ToEdit(set))
            throw new SecurityException("Not allowed to edit set");

        if (TempData["createSetMsg"] != null)
            model.Message = (SuccessMessage) TempData["createSetMsg"];

        return View(_viewLocation, model);
    }

    [HttpPost]
    [SetMenu(MenuEntry.None)]
    [SetThemeMenu(isQuestionSetPage: true)]
    public ViewResult Edit(int id, EditSetModel model)
    {
        var setRepo = Resolve<SetRepo>();
        var set = setRepo.GetById(id);

        if (!IsAllowedTo.ToEdit(set))
            throw new SecurityException("Not allowed to edit set");

        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set, HistoryItemType.Edit));
        StoreImage(set.Id);
        model.Fill(set);
        model.SetToUpdateModel();
        setRepo.Update(set);

        model.Message = new SuccessMessage("Das Lernset wurde gespeichert");
        
        return View(_viewLocation, model);
    }

    private void StoreImage(int setId)
    {
        if (Request["ImageIsNew"] == "true")
        {
            if (Request["ImageSource"] == "wikimedia")
            {
                Resolve<ImageStore>().RunWikimedia<SetImageSettings>(
                    Request["ImageWikiFileName"], setId, ImageType.QuestionSet, _sessionUser.User.Id);
            }
            if (Request["ImageSource"] == "upload")
            {
                Resolve<ImageStore>().RunUploaded<SetImageSettings>(
                    _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), setId, _sessionUser.User.Id,Request["ImageLicenseOwner"]);
            }
        }
    }

    public EmptyResult UpdateQuestionsOrder(int questionSetId, string newIndicies)
    {
        Resolve<UpdateQuestionsOrder>().Run(
            JsonConvert.DeserializeObject<IEnumerable<NewQuestionIndex>>(newIndicies)
        );
        
        return new EmptyResult();
    }

    [HttpPost]
    public EmptyResult RemoveQuestionFromSet(int questionInSetId)
    {
        Resolve<QuestionInSetRepo>().Delete(questionInSetId);
        return new EmptyResult();
    }
    
    public JsonResult Search(string term, int setId)
    {
        var searchSpec = new QuestionSearchSpec();
        searchSpec.Filter.SearchTerm = term;            
        searchSpec.PageSize = 5;

        var set = Sl.SetRepo.GetById(setId);
        set.QuestionIds().ForEach(questionId => searchSpec.Filter.QuestionIdsToExclude.Add(questionId));

        var searchResult = Sl.SearchQuestions.Run(searchSpec);

        var questions = searchResult.GetQuestions();

        return Json(new
        {
            Questions = questions.Select(question => new
            {
                Id = question.Id,
                question = question.Text,
                correctAnswer= question.GetSolution().GetCorrectAnswerAsHtml(),
                ImageUrl = new QuestionImageSettings(question.Id).GetUrl_50px_square().Url,
                QuestionUrl = Links.AnswerQuestion(question)
            })
        });
    }

    public JsonResult AddQuestionsToSet(int setId, List<int> questionIds)
    {
        try
        {
            AddToSet.Run(questionIds.ToArray(), setId);
        }
        catch(Exception e)
        {
            Logg.Error(e);
            return Json(new { Status = false, Message =  e.Message});
        }
        
        return Json(new {Status = true});
    }



    public JsonResult GetHtmlRows(List<int> questionIds, int setid)
    {
        var questionsInSet = Sl.QuestionInSetRepo.GetByQuestionIds(questionIds, setid);

        var resultHtmls = new List<string>();

        foreach (var questionInSet in questionsInSet)
            resultHtmls.Add(ViewRenderer.RenderPartialView("~/Views/Sets/Edit/QuestionInSet.ascx",
                new QuestionInSetModel(questionInSet), ControllerContext));

        return Json(resultHtmls);
    }

   

}
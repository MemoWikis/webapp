﻿using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using TrueOrFalse.Web;

public class EditSetController : BaseController
{
    private const string _viewLocation = "~/Views/Sets/Edit/EditSet.aspx";

    public ActionResult Create()
    {
        var model = new EditSetModel();
        model.SetToCreateModel();
        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditSetModel model)
    {
        if (!ModelState.IsValid){
            model.Message = new ErrorMessage(ModelState);
            return View(_viewLocation, model);
        }

        var set = model.ToQuestionSet();
        set.Creator = _sessionUser.User;
        Resolve<SetRepo>().Create(set);

        model = new EditSetModel();
        model.SetToCreateModel();
        model.Message = new SuccessMessage("Der Fragesatz wurde gespeichert, " +
                                           "du kannst nun einen neuen Fragesatz erstellen.");

        StoreImage(set.Id);

        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var set = Resolve<SetRepo>().GetById(id);
        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set, HistoryItemType.Edit));
        var model = new EditSetModel(set);
        model.SetToUpdateModel();
        return View(_viewLocation, model);
    }

    [HttpPost]
    public ViewResult Edit(int id, EditSetModel model)
    {
        var questionSetRepo = Resolve<SetRepo>();
        var set = questionSetRepo.GetById(id);
        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set, HistoryItemType.Edit));
        StoreImage(set.Id);
        model.Fill(set);
        model.SetToUpdateModel();
        questionSetRepo.Update(set);

        model.Message = new SuccessMessage("Der Fragesatz wurde gespeichert");
        
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
}
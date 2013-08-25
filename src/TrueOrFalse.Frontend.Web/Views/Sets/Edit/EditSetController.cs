﻿using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;
using Newtonsoft.Json;

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
        if (ModelState.IsValid)
        {
            var questionSet = model.ToQuestionSet();
            questionSet.Creator = _sessionUser.User;
            Resolve<SetRepository>().Create(questionSet);

            model = new EditSetModel();
            model.SetToCreateModel();
            model.Message = new SuccessMessage("Der Fragesatz wurde gespeichert, " +
                                               "nun kannst Du einen neuen Fragesatz erstellen.");

            StoreImage(questionSet.Id);

            return View(_viewLocation, model);
        }
        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var set = Resolve<SetRepository>().GetById(id);
        var model = new EditSetModel(set);
        model.SetToUpdateModel();
        return View(_viewLocation, model);
    }

    [HttpPost]
    public ViewResult Edit(int id, EditSetModel model)
    {
        var questionSetRepo = Resolve<SetRepository>();
        var questionSet = questionSetRepo.GetById(id);
        StoreImage(questionSet.Id);
        model.Fill(questionSet);
        model.SetToUpdateModel();
        questionSetRepo.Update(questionSet);

        model.Message = new SuccessMessage("Der Fragesatz wurde gespeichert");
        
        return View(_viewLocation, model);
    }

    private void StoreImage(int setId)
    {
        if (Request["ImageIsNew"] == "true")
        {
            if (Request["ImageSource"] == "wikimedia")
            {
                Resolve<ImageStore>().RunWikimedia<QuestionSetImageSettings>(
                    Request["ImageWikiFileName"], setId, _sessionUser.User.Id);
            }
            if (Request["ImageSource"] == "upload")
            {
                Resolve<ImageStore>().RunUploaded<QuestionSetImageSettings>(
                    _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), setId, _sessionUser.User.Id,Request["ImageLicenceOwner"]);
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
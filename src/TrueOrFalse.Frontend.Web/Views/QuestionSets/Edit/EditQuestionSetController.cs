using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FineUploader;
using TrueOrFalse;
using TrueOrFalse.Web;

public class EditQuestionSetController : BaseController
{
    private const string _viewLocation = "~/Views/QuestionSets/Edit/EditQuestionSet.aspx";

    public ActionResult Create()
    {
        var model = new EditQuestionSetModel();
        model.SetToCreateModel();
        return View(_viewLocation, model);
    }

    [HttpPost]
    public ActionResult Create(EditQuestionSetModel model)
    {
        if (ModelState.IsValid)
        {
            var questionSet = model.ToQuestionSet();
            questionSet.Creator = _sessionUser.User;
            Resolve<QuestionSetRepository>().Create(questionSet);

            model = new EditQuestionSetModel();
            model .SetToCreateModel();
            model.Message = new SuccessMessage("Der Fragesatz wurde gespeichert, " +
                                               "nun kannst Du einen neuen Fragesatz erstellen.");

            return View(_viewLocation, model);
        }
        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var set = Resolve<QuestionSetRepository>().GetById(id);
        var model = new EditQuestionSetModel(set);
        model.SetToUpdateModel();
        return View(_viewLocation, model);
    }

    [HttpPost]
    public ViewResult Edit(int id, EditQuestionSetModel model)
    {
        var questionSetRepo = Resolve<QuestionSetRepository>();
        var questionSet = questionSetRepo.GetById(id);
        model.Fill(questionSet);
        model.SetToUpdateModel();
        questionSetRepo.Update(questionSet);

        model.Message = new SuccessMessage("Der Fragesatz wurde gespeichert");
        
        return View(_viewLocation, model);
    }

    public ActionResult Update()
    {
        return View(_viewLocation, new EditQuestionSetModel());
    }

    //public FineUploaderResult UploadImage(FineUpload upload, string extraParam1, int extraParam2)
    [HttpPost]
    public FineUploaderResult UploadImage(int id, FineUpload upload)
    {
        // asp.net mvc will set extraParam1 and extraParam2 from the params object passed by Fine-Uploader
        var dir = @"c:\upload\path";
        var filePath = Path.Combine(dir, upload.Filename);
        try { upload.SaveAs(filePath); }
        catch (Exception ex){return new FineUploaderResult(false, error: ex.Message);}

        // the anonymous object in the result below will be convert to json and set back to the browser
        return new FineUploaderResult(true, new { extraInformation = 12345 });
    }
}
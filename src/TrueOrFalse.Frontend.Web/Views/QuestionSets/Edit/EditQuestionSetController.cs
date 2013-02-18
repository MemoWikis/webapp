using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
}
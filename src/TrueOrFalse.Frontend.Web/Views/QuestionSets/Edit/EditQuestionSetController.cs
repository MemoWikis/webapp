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

    public ActionResult Create(){
        return View(_viewLocation, new EditQuestionSetModel());
    }

    [HttpPost]
    public ActionResult Create(EditQuestionSetModel model)
    {
        if (ModelState.IsValid)
        {
            var questionSet = model.ToQuestionSet();
            questionSet.Creator = _sessionUser.User;
            Resolve<QuestionSetRepository>().Create(questionSet);
            model.Message = new SuccessMessage("Fragesatz wurde gespeichert");
        }

        return View(_viewLocation, model);
    }

    public ViewResult Edit(int id)
    {
        var set = Resolve<QuestionSetRepository>().GetById(id);
        return View(_viewLocation, new EditQuestionSetModel(set));
    }

    [HttpPost]
    public ViewResult Edit(int id, EditQuestionSetModel setModel)
    {
        return View(_viewLocation, setModel);
    }

    public ActionResult Update()
    {
        return View(_viewLocation, new EditQuestionSetModel());
    }
}
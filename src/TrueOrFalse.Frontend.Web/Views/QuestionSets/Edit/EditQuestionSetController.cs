using System.Collections.Generic;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;
using Newtonsoft.Json;

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
        StoreImage(questionSet.Id);
        model.Fill(questionSet);
        model.SetToUpdateModel();
        questionSetRepo.Update(questionSet);

        model.Message = new SuccessMessage("Der Fragesatz wurde gespeichert");
        
        return View(_viewLocation, model);
    }

    private void StoreImage(int questionId)
    {
        if (Request["ImageIsNew"] == "true")
        {
            if (Request["ImageSource"] == "wikimedia")
            {
                Resolve<QuestionSetImageStore>().RunWikimedia(
                    Request["ImageWikiFileName"], questionId, _sessionUser.User.Id);
            }
            if (Request["ImageSource"] == "upload")
            {
                Resolve<QuestionSetImageStore>().RunUploaded(
                    _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), questionId, _sessionUser.User.Id,Request["ImageLicenceOwner"]);
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
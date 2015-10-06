using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using TrueOrFalse.Frontend.Web.Code;
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

        StoreImage(set.Id);

        model = new EditSetModel(set);
        model.SetToUpdateModel();

        TempData["createSetMsg"] = new SuccessMessage(
            string.Format("Der Fragesatz <i>'{0}'</i> wurde erstellt. Du kannst ihn nun weiter bearbeiten.",
                          set.Name.TruncateAtWord(30)));

        return RedirectToAction("Edit", Links.EditSetController, new {set.Id});
    }

    public ViewResult Edit(int id)
    {
        var set = Resolve<SetRepo>().GetById(id);
        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set, HistoryItemType.Edit));
        var model = new EditSetModel(set);
        model.SetToUpdateModel();

        if (TempData["createSetMsg"] != null)
        {
            model.Message = (SuccessMessage)TempData["createSetMsg"];
        }

        return View(_viewLocation, model);
    }

    [HttpPost]
    public ViewResult Edit(int id, EditSetModel model)
    {
        var setRepo = Resolve<SetRepo>();
        var set = setRepo.GetById(id);
        _sessionUiData.VisitedSets.Add(new QuestionSetHistoryItem(set, HistoryItemType.Edit));
        StoreImage(set.Id);
        model.Fill(set);
        model.SetToUpdateModel();
        setRepo.Update(set);

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
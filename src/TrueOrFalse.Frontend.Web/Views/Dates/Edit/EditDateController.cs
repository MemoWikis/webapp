using System;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web;

[SetMenu(MenuEntry.Dates)]
public class EditDateController : BaseController
{
    private const string _viewLocation = "~/Views/Dates/Edit/EditDate.aspx";

    [HttpGet]
    public ViewResult Create()
    {
        return View(_viewLocation, new EditDateModel());
    }

    [HttpPost]
    public ViewResult Create(EditDateModel model)
    {
        if (model.IsDateTimeInPast())
            model.Message = new ErrorMessage("Es wurde kein Termin gespeichert. Der Termin darf nicht in der Vergangenheit liegen. Bitte prüfe das Datum.");

        if (!model.HasSets())
            model.Message = new ErrorMessage("Der Termin konnte nicht gespeichert werden. Bitte füge mindestens einen Fragesatze hinzu.");

        if (!model.HasErrorMsg())
        {
            var date = model.ToDate();
            R<DateRepo>().Create(date);

            _sessionUiData.VisitedDatePages.Add(new DateHistoryItem(date, HistoryItemType.Edit));

            CareAboutAnswerProbability(date);

            Response.Redirect("/Termine", true);
        }

        return View(_viewLocation, model);
    }

    [HttpGet]
    public ViewResult Edit(int dateId)
    {
        var date = R<DateRepo>().GetById(dateId);

        _sessionUiData.VisitedDatePages.Add(new DateHistoryItem(date, HistoryItemType.Edit));

        if (!_sessionUser.IsLoggedInUser(date.User.Id))
            throw new Exception("Invalid exception");

        return View(_viewLocation, model: new EditDateModel(date));
    }

    [HttpPost]
    public ViewResult Edit(EditDateModel model)
    {
        if (!_sessionUser.IsLoggedInUser(model.UserId))
            throw new Exception("Invalid exception");

        if (model.IsDateTimeInPast())
            model.Message = new ErrorMessage("Es wurde kein Termin gespeichert. Der Termin darf nicht in der Vergangenheit liegen Bitte prüfe das Datum.");

        if(!model.HasSets())
            model.Message = new ErrorMessage("Der Termin konnte nicht gespeichert werden. Bitte füge mindestens einen Fragesatze hinzu.");
        else
            model.FillSetsFromInput();

        if (!model.HasErrorMsg())
        {
            var dateRepo = R<DateRepo>();
            var date = dateRepo.GetById(model.DateId);

            dateRepo.Update(model.FillDateFromInput(date));
            dateRepo.Flush();

            CareAboutAnswerProbability(date);

            Response.Redirect("/Termine", true);
        }

        model.IsEditing = true;
        return View(_viewLocation, model);
    }

    private void CareAboutAnswerProbability(Date date)
    {
        R<AddProbabilitiesEntries_ForSetsAndDates>().Run(date.Sets, _sessionUser.User);
        R<ProbabilityUpdate_Valuation>().Run(date.Sets, _sessionUser.UserId);
    }
}
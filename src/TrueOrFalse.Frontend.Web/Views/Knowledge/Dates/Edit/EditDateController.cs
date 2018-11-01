using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NHibernate.Mapping;
using Seedworks.Web.State;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;
using Array = NHibernate.Mapping.Array;


public class EditDateController : BaseController
{
private const string _viewLocation = "~/Views/Knowledge/Dates/Edit/EditDate.ascx";

    [HttpPost]
    public string CreateNew(int? setId = null, int? categoryId = null, List<int> setIds = null, string setListTitle = null, string timeVar =null, string dateVar = null, string nameOfDate = null )
    {

       // var result = JsonConvert.DeserializeObject<string>(setIdsArray);
        var model = new EditDateModel();
        CultureInfo fc = new CultureInfo("de-DE");
        model.Date = DateTime.ParseExact(dateVar,"dd.MM.yyyy", fc, DateTimeStyles.None);
        model.Time = timeVar;
        model.Details = nameOfDate;

        if (setId != null)
        {
            var set = Sl.R<SetRepo>().GetById((int)setId);
            if (set != null)
            {
                model.Details = set.Name;
                model.Sets.Add(set);
            }
        }
        else if (setIds != null)
        {
            var sets = Sl.R<SetRepo>().GetByIds(setIds);
            ((List<Set>)model.Sets).AddRange(sets);
        }
        else if (categoryId != null)
        {
            var category = Sl.R<CategoryRepository>().GetById((int)categoryId);
            var sets = category.GetAggregatedSetsFromMemoryCache();
            if (sets != null)
            {
                model.Details = category.Name;
                model.Sets = sets;
            }
        }

        return Create(model);
    }

    [HttpPost]
    public string Create()
    {
        var model = new EditDateModel();
        return ViewRenderer.RenderPartialView(_viewLocation, model, ControllerContext);
    }

    [HttpPost]
    private string Create(EditDateModel model)
    {
        if (model.IsDateTimeInPast())
            model.Message =
                new ErrorMessage(
                    "Es wurde kein Termin gespeichert. Der Termin darf nicht in der Vergangenheit liegen. Bitte prüfe das Datum.");

        if (!model.HasQuestions())
            model.Message =
                new ErrorMessage(
                    "Der Termin konnte nicht gespeichert werden, da er keine Fragen enthält. Bitte füge mindestens ein Lernset hinzu, das nicht leer ist.");

        if (!model.HasErrorMsg())
        {
            var date = model.ToDate();

            R<DateRepo>().CreateWithTrainingPlan(date);

            _sessionUiData.VisitedDatePages.Add(new DateHistoryItem(date, HistoryItemType.Edit));

            CareAboutAnswerProbability(date);

            Response.Redirect(Links.Dates(), true);
        }

        return ViewRenderer.RenderPartialView(_viewLocation, model, ControllerContext);
    }

    //    [HttpGet]
    //    [SetMenu(MenuEntry.DateDetail)]
    //    public ViewResult Edit(int dateId)
    //    {
    //        var date = R<DateRepo>().GetById(dateId);

    //        _sessionUiData.VisitedDatePages.Add(new DateHistoryItem(date, HistoryItemType.Edit));

    //        if (!_sessionUser.IsLoggedInUser(date.User.Id))
    //            throw new Exception("Invalid exception");

    //        return View(_viewLocation, model: new EditDateModel(date));
    //    }

    //    [HttpPost]
    //    [SetMenu(MenuEntry.DateDetail)]
    //    public ViewResult Edit(EditDateModel model)
    //    {
    //        if (!_sessionUser.IsLoggedInUser(model.UserId))
    //            throw new Exception("Invalid exception");

    //        if (model.IsDateTimeInPast())
    //            model.Message = new ErrorMessage("Es wurde kein Termin gespeichert. Der Termin darf nicht in der Vergangenheit liegen Bitte prüfe das Datum.");

    //        if(!model.HasSets())
    //            model.Message = new ErrorMessage("Der Termin konnte nicht gespeichert werden. Bitte füge mindestens ein Lernset hinzu.");
    //        else
    //            model.FillSetsFromInput();

    //        if (!model.HasErrorMsg())
    //        {
    //            var dateRepo = R<DateRepo>();
    //            var date = dateRepo.GetById(model.DateId);

    //            dateRepo.UpdateWithTrainingPlan(model.FillDateFromInput(date));

    //            CareAboutAnswerProbability(date);

    //            Response.Redirect(Links.Dates(), true);
    //        }

    //        model.IsEditing = true;
    //        return View(_viewLocation, model);
    //    }

    private void CareAboutAnswerProbability(Date date)
    {
        R<AddValuationEntries_ForQuestionsInSetsAndDates>().Run(date.Sets, _sessionUser.User);
        ProbabilityUpdate_Valuation.Run(date.Sets, _sessionUser.UserId);
    }
}

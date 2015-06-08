using System;
using System.Web.Mvc;

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
        var date = model.ToDate();
        R<DateRepo>().Create(date);

        Response.Redirect("/Termine", true);

        return View(_viewLocation, new EditDateModel());
    }

    [HttpGet]
    public ViewResult Edit(int dateId)
    {
        var date = R<DateRepo>().GetById(dateId);
        
        if(!_sessionUser.IsValidUserOrAdmin(date.User.Id))
            throw new Exception("Invalid exception");

        return View(_viewLocation, model: new EditDateModel(date));
    }

    [HttpPost]
    public ViewResult Edit(EditDateModel model)
    {
        var dateRepo = R<DateRepo>();
        var date = dateRepo.GetById(model.DateId);

        dateRepo.Update(model.FillDateFromInput(date));
        dateRepo.Flush();

        Response.Redirect("/Termine", true);

        return View(_viewLocation, model);
    }
}
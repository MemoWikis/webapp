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
        var date = new Date();
        date.Sets = AutocompleteUtils.GetReleatedSetsFromPostData(Request.Form);
        date.Details = model.Details;
        date.DateTime = Time.Parse(model.Time).SetTime(model.Date);
        date.User = _sessionUser.User;

        if (model.Visibility == "inNetwork")
            date.Visibility = DateVisibility.InNetwork;
        else if (model.Visibility == "private")
            date.Visibility = DateVisibility.Private;
        else
            throw new Exception("Invalid mapping");

        R<DateRepo>().Create(date);

        Response.Redirect("/Termine", true);

        return View(_viewLocation, new EditDateModel());
    }

    public ViewResult Edit()
    {
        return View(_viewLocation, new EditDateModel());        
    }
}
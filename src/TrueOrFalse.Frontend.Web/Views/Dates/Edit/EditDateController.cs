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

    public ViewResult Edit()
    {
        return View(_viewLocation, new EditDateModel());        
    }
}
using System.Web.Mvc;

public class DatesController : BaseController
{
    [SetMenu(MenuEntry.Dates)]
    public ActionResult Dates()
    {
        return View(new DatesModel());
    }

    [HttpPost]
    public JsonResult DeleteDetails(int id)
    {
        var date = R<DateRepo>().GetById(id);

        return new JsonResult{
            Data = new{
                DateInfo = date.GetInfo(),
            }
        };        
    }

    [HttpPost]
    public EmptyResult Delete(int id)
    {
        R<DeleteDate>().Run(id);

        return new EmptyResult();
    }
}
using System.IO;
using System.Net;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class ExportController : BaseController
{
    private readonly QuestionRepo _questionRepo;
    private readonly CategoryRepository _categoryRepository;

    public ExportController(QuestionRepo questionRepo, 
                            CategoryRepository categoryRepository)
    {
        _questionRepo = questionRepo;
        _categoryRepository = categoryRepository;
    }

    public ViewResult Export()
    {
        const string viewLocation = "~/Views/Api/Export.aspx";
        var model = new ExportModel(_questionRepo.GetAll(), _categoryRepository.GetAll());
        return View(viewLocation, model);
    }
    [AccessOnlyAsAdmin]
    public ActionResult AllToLocalFile()
    {
        ExportToFile("Export", Server.MapPath("~/SampleData/Export.xml"));
        return RedirectToAction(Links.KnowledgeAction, Links.KnowledgeController);
    }
            
    private void ExportToFile(string actionName , string filePath)
    {
        var request = WebRequest.Create(Url.Action(actionName, "Export", null, "http"));

        using (var responseStream = request.GetResponse().GetResponseStream())
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                responseStream.CopyTo(fileStream);
            }
    }
}
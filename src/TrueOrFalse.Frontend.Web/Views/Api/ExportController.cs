using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;


public class ExportController : Controller
{
    private readonly QuestionRepository _questionRepository;
    private readonly CategoryRepository _categoryRepository;

    public ExportController(QuestionRepository questionRepository, 
                            CategoryRepository categoryRepository)
    {
        _questionRepository = questionRepository;
        _categoryRepository = categoryRepository;
    }

    public ViewResult Export()
    {
        const string viewLocation = "~/Views/Api/Export.aspx";
        var model = new ExportModel(_questionRepository.GetAll(), _categoryRepository.GetAll());
        return View(viewLocation, model);
    }
    [AccessOnlyAsAdmin]
    public ActionResult AllToLocalFile()
    {
        ExportToFile("Export", Server.MapPath("~/SampleData/Export.xml"));
        return RedirectToAction(Links.Knowledge, Links.KnowledgeController);
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
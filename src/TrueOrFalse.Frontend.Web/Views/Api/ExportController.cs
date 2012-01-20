using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TrueOrFalse.Core;
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

    public ViewResult Questions()
    {
        const string viewLocation = "~/Views/Api/ExportQuestions.aspx";
        var model = new ExportQuestionsModel(_questionRepository.GetAll());
        return View(viewLocation, model);
    }

    public ViewResult Categories()
    {
        const string viewLocation = "~/Views/Api/ExportCategories.aspx";
        var model = new ExportCategoriesModel(_categoryRepository.GetAll());
        return View(viewLocation, model);
    }

    [AccessOnlyLocalAttribute]
    public ActionResult AllToLocalFile()
    {
        ExportToFile("Questions", Server.MapPath("~/SampleData/Questions.xml"));
        ExportToFile("Categories", Server.MapPath("~/SampleData/Categories.xml"));
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
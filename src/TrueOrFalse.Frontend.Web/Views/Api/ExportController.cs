using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Core;


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

    public ActionResult Questions()
    {
        const string viewLocation = "~/Views/Api/ExportQuestions.aspx";
        var model = new ExportQuestionsModel(_questionRepository.GetAll());
        return View(viewLocation, model);
    }

    public ActionResult Categories()
    {
        const string viewLocation = "~/Views/Api/ExportCategories.aspx";
        var model = new ExportCategoriesModel(_categoryRepository.GetAll());
        return View(viewLocation, model);
    }
}
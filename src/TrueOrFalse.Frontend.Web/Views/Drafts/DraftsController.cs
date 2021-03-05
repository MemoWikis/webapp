using System.Web.Mvc;

public class DraftsController : BaseController
{
    [AccessOnlyAsAdmin]
    public ActionResult Bootstrap()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Boxes()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult FontAwesome()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Forms()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Grid()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Icons()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult ContentUnits()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult RangeSlider()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Reference()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult temp()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult Templates()
    {
        return View(new WelcomeModel());
    }

    [AccessOnlyAsAdmin]
    public ActionResult CategoryPartials()
    {
       
        var category = Sl.R<CategoryRepository>().GetById(145);
        category.TopicMarkdown =
@"[[{ ""TemplateName"":""DivStart"", ""CssClasses"":""Box""}]]
#### Box-Inhalt
[[{""TemplateName"":""DivEnd""}]]
[[{""TemplateName"":""DivStart"", ""CssClasses"":""row CardsLandscape""}]]
[[{""TemplateName"":""SetListCard"",
""Title"":""Einbürgerungstest für Thüringen"",
""Description"":""Lerne die allgemeinen Fragen und die landesspezifischen Fragen für Thüringen"",
""SetListIds"":""27,9,48""}]]
[[{""TemplateName"":""SetListCard"",
""Title"":""Einbürgerungstest für Berlin"",
""TitleRowCount"": ""2"",
""Description"":""TestDescription"",
""DescriptionRowCount"":0,
""SetListIds"":""27,49"",
""RowCount"":3}]]
[[{""TemplateName"":""DivEnd""}]]
[[{""TemplateName"":""CategoryNetwork""}]]
[[{""TemplateName"":""ContentLists""}]]

#### Die besten Lernsets dieses Themas
[[{""TemplateName"":""DivStart"", ""CssClasses"":""row CardsPortrait""}]]
[[{""TemplateName"": ""SingleSet"",
""SetId"": 3,
""SetText"": ""test set text""}]]
[[{""TemplateName"": ""SingleSet"",
""SetId"": 7}]]
[[{""TemplateName"": ""SingleSet"",
""SetId"": 9}]]
[[{""TemplateName"":""DivEnd""}]]";

        var contentHtml = string.IsNullOrEmpty(category.TopicMarkdown)
           ? null
           : TemplateToHtml.Run(category, ControllerContext);


        return View(new CategoryModel(category)
        {
            CustomPageHtml = contentHtml
        });
    }
}
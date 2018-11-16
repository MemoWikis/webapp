using System.Web.Mvc;
using MarkdownSharp;

public class TopicsController : BaseController
{
    [SetMainMenu(MainMenuEntry.None)]
    public ActionResult Topic(string topicName)
    {
        var topicPath = Server.MapPath($"~/Views/Topics/Docs/{topicName}.md");

        var content = System.IO.File.Exists(topicPath) 
            ? new Markdown().Transform(System.IO.File.ReadAllText(topicPath)) 
            : "Topic not found";

        return View("~/Views/Topics/Topic.aspx", new TopicModel {Content = content});
    }
}

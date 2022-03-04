using System.IO;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class GetQuestionSoundUrl
{
    public string Run(QuestionCacheItem question)
    {
        const string relativePath = "/Sounds/Questions/";
        var serverPath = HttpContext.Current.Server.MapPath(relativePath);
        var file = Directory.GetFiles(serverPath, string.Format("{0}.*", question.Id)).SingleOrDefault();
        if (file == null) return "";
        return relativePath + Path.GetFileName(file);
    }
}
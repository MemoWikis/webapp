using System;
using System.IO;
using System.Linq;
using System.Web;
using TrueOrFalse.Core;

public class GetQuestionImageUrl : GetImageUrl<Question>
{
    protected override string RelativePath
    {
        get { return "/Images/Questions/"; }
    }

    protected override string GetFallbackImage(Question entity)
    {
        return "";
    }
}


public class GetQuestionSoundUrl
{
    public string Run(Question question)
    {
        const string relativePath = "/Sounds/Questions/";
        var serverPath = HttpContext.Current.Server.MapPath(relativePath);
        var file = Directory.GetFiles(serverPath, string.Format("{0}.*", question.Id)).SingleOrDefault();
        if (file == null) return "";
        return relativePath + Path.GetFileName(file);
    }
}
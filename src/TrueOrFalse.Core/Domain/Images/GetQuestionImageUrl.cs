using System;
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
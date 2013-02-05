using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class QuestionSetRowModel
{
    public int Id;
    public string Name;
    public string DescriptionShort;

    public int IndexInResult;

    public int QuestionCount;

    public Func<UrlHelper, string> DetailLink { get; private set; }
    public Func<UrlHelper, string> UserProfileLink { get; private set; }

    public string CreatorName;
    public int CreatorId;
    public bool IsOwner;

    public QuestionSetRowModel(QuestionSet questionSet, int indexInResultSet, int currentUserid)
    {
        Id = questionSet.Id;
        Name = questionSet.Name;
        CreatorId = questionSet.Creator.Id;

        DescriptionShort = questionSet.Text.Wrap(150);
        
        QuestionCount = questionSet.Questions.Count;
        CreatorName = questionSet.Creator.Name;
        IsOwner = currentUserid == questionSet.Creator.Id;
        IndexInResult = indexInResultSet;

        DetailLink = urlHelper => Links.QuestionSetDetail(urlHelper, questionSet, indexInResultSet);
        UserProfileLink = urlHelper => Links.Profile(urlHelper, questionSet.Creator.Name, questionSet.Creator.Id);
    }

}
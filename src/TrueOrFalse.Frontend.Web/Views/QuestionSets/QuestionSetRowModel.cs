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

    public Func<UrlHelper, string> DetailLink;
    public Func<UrlHelper, string> UserLink;

    public string ImageUrl;

    public string CreatorName;
    public int CreatorId;
    public bool IsOwner;

    public QuestionSetRowModel(QuestionSet questionSet, int indexInResultSet, int currentUserid)
    {
        Id = questionSet.Id;
        Name = questionSet.Name;
        CreatorId = questionSet.Creator.Id;

        DescriptionShort = questionSet.Text.Wrap(150);
        
        QuestionCount = questionSet.QuestionsInSet.Count;
        CreatorName = questionSet.Creator.Name;
        IsOwner = currentUserid == questionSet.Creator.Id;
        IndexInResult = indexInResultSet;

        DetailLink = urlHelper => Links.QuestionSetDetail(urlHelper, questionSet, indexInResultSet);
        UserLink = urlHelper => Links.UserDetail(urlHelper, questionSet.Creator.Name, questionSet.Creator.Id);

        ImageUrl = new QuestionSetImageSettings(questionSet.Id).GetUrl_206px_square().Url;
    }

}
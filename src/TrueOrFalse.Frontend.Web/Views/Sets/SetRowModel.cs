using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class SetRowModel
{
    public int Id;
    public string Name;
    public string DescriptionShort;

    public int IndexInResult;

    public int QuestionCount;
    public IList<Category> Categories; 

    public Func<UrlHelper, string> DetailLink;
    public Func<UrlHelper, string> UserLink;

    public string ImageUrl;

    public string CreatorName;
    public int CreatorId;
    public bool IsOwner;


    public SetRowModel(Set set, int indexInResultSet, int currentUserid)
    {
        Id = set.Id;
        Name = set.Name;
        CreatorId = set.Creator.Id;

        DescriptionShort = set.Text.Wrap(150);
        
        QuestionCount = set.QuestionsInSet.Count;
        CreatorName = set.Creator.Name;
        IsOwner = currentUserid == set.Creator.Id;
        IndexInResult = indexInResultSet;

        DetailLink = urlHelper => Links.QuestionSetDetail(urlHelper, set, indexInResultSet);
        UserLink = urlHelper => Links.UserDetail(urlHelper, set.Creator.Name, set.Creator.Id);

        ImageUrl = QuestionSetImageSettings.Create(set.Id).GetUrl_128px_square().Url;
        Categories = set.Categories;
    }

}
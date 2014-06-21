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

    public int RelevancePersonal;
    public string TotalRelevancePersonalEntries;
    public string TotalRelevancePersonalAvg;

    public bool IsInWishknowledge;

    public SetRowModel(
        Set set, 
        SetValuation setValuation,
        int indexInResultSet, 
        int currentUserid)
    {
        Id = set.Id;
        Name = set.Name;
        CreatorId = set.Creator.Id;

        DescriptionShort = set.Text.Wrap(150);
        
        QuestionCount = set.QuestionsInSet.Count;
        CreatorName = set.Creator.Name;
        IsOwner = currentUserid == set.Creator.Id;
        IndexInResult = indexInResultSet;

        RelevancePersonal = setValuation.RelevancePersonal;

        IsInWishknowledge = setValuation.IsInWishknowledge();

        TotalRelevancePersonalEntries = set.TotalRelevancePersonalEntries.ToString();
        TotalRelevancePersonalAvg = (set.TotalRelevancePersonalAvg / 10d).ToString();

        DetailLink = urlHelper => Links.SetDetail(urlHelper, set, indexInResultSet);
        UserLink = urlHelper => Links.UserDetail(urlHelper, set.Creator.Name, set.Creator.Id);

        ImageUrl = QuestionSetImageSettings.Create(set.Id).GetUrl_128px_square().Url;
        Categories = set.Categories;
    }

}
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse.Frontend.Web.Code;

public class SetRowModel
{
    public int Id;
    public string Name;
    public string DescriptionShort;

    public bool HasVideo;
    public int QuestionCount;
    public IList<Category> Categories; 

    public Func<UrlHelper, string> DetailLink;
    public Func<UrlHelper, string> UserLink;

    public ImageFrontendData ImageFrontendData;

    public string CreatorName;
    public int CreatorId;
    public bool IsOwner;

    public int RelevancePersonal;
    public string TotalPins;

    public bool IsInWishknowledge;

    public SetRowModel(Set set,  SetValuation setValuation, int currentUserid)
    {
        Id = set.Id;
        Name = set.Name;
        CreatorId = set.Creator.Id;

        DescriptionShort = !String.IsNullOrEmpty(set.Text) ? (set.Text.Wrap(150)) : "";

        HasVideo = set.HasVideo;
        QuestionCount = set.QuestionsInSet.Count;
        CreatorName = set.Creator.Name;
        IsOwner = currentUserid == set.Creator.Id;

        var imageMetaData = ServiceLocator.Resolve<ImageMetaDataRepo>().GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        RelevancePersonal = setValuation.RelevancePersonal;
        IsInWishknowledge = setValuation.IsInWishKnowledge();

        TotalPins = set.TotalRelevancePersonalEntries.ToString();

        DetailLink = urlHelper => Links.SetDetail(urlHelper, set);
        UserLink = urlHelper => Links.UserDetail(set.Creator.Name, set.Creator.Id);

        Categories = set.Categories;
    }

}
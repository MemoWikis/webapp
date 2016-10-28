﻿using System;
using System.Net.Mime;
using System.Web.Mvc;
using NHibernate.Util;
using TrueOrFalse.Frontend.Web.Code;

public class WelcomeBoxSingleSetModel : BaseModel
{
    public int SetId;
    public string SetName;
    public string SetText;
    public int QCount; //Number of questions in questionset
    public int FirstQId;
    public string FirstQText;
    public Func<UrlHelper, string> SetDetailLink;

    public bool IsInWishknowledge;

    public ImageFrontendData ImageFrontendData;

    public WelcomeBoxSingleSetModel(int setId, string setText = null)
    {
        var set = Resolve<SetRepo>().GetById(setId) ?? new Set();
        
        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        SetId = set.Id;
        SetName = set.Name;
        SetText = setText ?? set.Text;
        SetDetailLink = urlHelper => Links.SetDetail(urlHelper, set);

        QCount = set.Questions().Count;
        if (set.Questions().Count > 0)
        {
            FirstQId = set.Questions()[0].Id;
            FirstQText = R<QuestionRepo>().GetById(FirstQId).Text; // this repo only needed to get Text of First Question to generate Link in Partial View!!
        }
        else
        {
            FirstQId = 0;
            FirstQText = "";
        }

        IsInWishknowledge = R<SetValuationRepo>().GetBy(setId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;

    }

    public static WelcomeBoxSingleSetModel GetWelcomeBoxSetSingleModel(int setId, string setText = null)
    {
        return new WelcomeBoxSingleSetModel(setId, setText);
    }
}

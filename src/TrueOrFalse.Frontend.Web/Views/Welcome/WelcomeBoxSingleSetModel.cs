using System;
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

        QCount = set.Questions().Count;

        IsInWishknowledge = R<SetValuationRepo>().GetBy(setId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;

    }

    public static WelcomeBoxSingleSetModel GetWelcomeBoxSetSingleModel(int setId, string setText = null)
    {
        return new WelcomeBoxSingleSetModel(setId, setText);
    }
}

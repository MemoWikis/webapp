using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Util;


public class SingleSetModel : BaseContentModule
{
    public Set Set;

    public int SetId;
    public string SetName;
    public string SetText;
    public int QCount; //Number of questions in questionset

    public bool IsInWishknowledge;

    public ImageFrontendData ImageFrontendData;
    
    public SingleSetModel(Set set) : this(new SingleSetJson(set))
    {
    }

    public SingleSetModel(SingleSetJson singleSetJson)
    {
        var set = Sl.R<SetRepo>().GetById(singleSetJson.SetId);

        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        Set = set;
        SetId = set.Id;
        SetName = set.Name;
        SetText = singleSetJson.SetText;

        QCount = set.Questions().Count;

        IsInWishknowledge = R<SetValuationRepo>().GetBy(SetId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;   
    }
}

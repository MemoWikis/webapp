using System;
using System.Collections.Generic;

public class CardsModel : BaseContentModule
{
    public IList<Set> Sets;

    public CardsModel(IList<Set> sets)
    {
        Sets = sets;
    }

    public ImageFrontendData GetSetImage(Set set)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(set.Id, ImageType.QuestionSet);
        return new ImageFrontendData(imageMetaData);
    }
}
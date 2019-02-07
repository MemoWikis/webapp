using System;
using System.Collections.Generic;

public class SetCardMiniListModel : BaseContentModule
{
    public IList<Set> Sets;

    public SetCardMiniListModel(SetCardMiniListJson setCardMiniListJson)
    {
        Sets = setCardMiniListJson.SetList;
    }

    public ImageFrontendData GetSetImage(Set set)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(set.Id, ImageType.QuestionSet);
        return new ImageFrontendData(imageMetaData);
    }
}
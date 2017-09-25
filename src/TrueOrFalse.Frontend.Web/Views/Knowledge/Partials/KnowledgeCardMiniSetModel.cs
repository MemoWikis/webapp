using System;

public class KnowledgeCardMiniSetModel
{
    public Set Set;
    public KnowledgeCardMiniSetModel(Set set)
    {
        Set = set;
    }

    public ImageFrontendData GetSetImage(Set set)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(set.Id, ImageType.QuestionSet);
        return new ImageFrontendData(imageMetaData);
    }
}
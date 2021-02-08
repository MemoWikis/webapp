public class KnowledgeCardMiniSetModel:BaseModel
{
    public Set Set;
    public bool isInWishKnowledge;

    public KnowledgeCardMiniSetModel(Set set)
    {
        Set = set;
        isInWishKnowledge = false;
    }

    public ImageFrontendData GetSetImage(Set set)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(set.Id, ImageType.QuestionSet);
        return new ImageFrontendData(imageMetaData);
    }
}
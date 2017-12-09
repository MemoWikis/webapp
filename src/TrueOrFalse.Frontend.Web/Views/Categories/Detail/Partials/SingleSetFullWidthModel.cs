using System;

public class SingleSetFullWidthModel : BaseModel
{
    public Set Set;
    public int SetId;
    public int CreatorId;
    public string Creator;
    public string Title;
    public string Text;
    public int QuestionCount;
    public ImageFrontendData ImageFrontendData;
    public bool IsInWishknowledge;

    public SingleSetFullWidthModel(int setId, string title = null, string text = null)
    {
        Set = Sl.SetRepo.GetById(setId);
        SetId = Set.Id;
        if (Set == null)
            throw new Exception("Die angegebene Lernset-ID verweist nicht auf ein existierendes Lernset.");

        CreatorId = Set.Creator.Id;
        Creator = Set.Creator.Name;
        Title = title ?? Set.Name;
        Text = text ?? Set.Text;
        QuestionCount = Set.QuestionsPublicCount();

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(Set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        IsInWishknowledge = R<SetValuationRepo>().GetBy(SetId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;
    }

}

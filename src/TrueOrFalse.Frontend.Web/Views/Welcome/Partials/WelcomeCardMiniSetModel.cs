public class WelcomeCardMiniSetModel : BaseModel
{
    public Set Set;
    public int SetId;
    public string SetName;
    public string SetText;
    public ImageFrontendData ImageFrontendData;

    public int QuestionCount;
    public bool IsInWishknowledge;


    public WelcomeCardMiniSetModel(int setId)
    {
        Set = R<SetRepo>().GetById(setId) ?? new Set();
        SetId = Set.Id;
        SetName = Set.Name;
        //SetText = Set.Text;

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(SetId, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

    }
}
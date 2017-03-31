public class WidgetSetStartModel : BaseModel
{
    public int SetId;
    public string SetName;
    public string SetText;
    //public ImageFrontendData ImageFrontendData;
    //public int QuestionCount;
    public bool HideAddToKnowledge;

    public WidgetSetStartModel(int setId, bool hideAddToKnowledge)
    {
        SetId = setId;
        var set = Sl.R<SetRepo>().GetById(setId);
        SetName = set.Name;
        SetText = set.Text;
        //QuestionCount = set.QuestionsInSet.Count;

        //var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(set.Id, ImageType.QuestionSet);
        //ImageFrontendData = new ImageFrontendData(imageMetaData);

        HideAddToKnowledge = hideAddToKnowledge;

        ShowUserReportWidget = false;
    }
}

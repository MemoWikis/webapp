using System.Net.Mime;
using NHibernate.Util;

public class WelcomeBoxSingleSetModel : BaseModel
{
    public int SetId;
    public string SetName;
    public string SetText;
    public int QCount; //Number of questions in questionset
    public int FirstQId;
    public string FirstQText;

    public ImageFrontendData ImageFrontendData;

    public WelcomeBoxSingleSetModel(int setId, string setText = null)
    {
        var set = Resolve<SetRepo>().GetById(setId) ?? new Set();
        
        var imageMetaData = Resolve<ImageMetaDataRepository>().GetBy(set.Id, ImageType.QuestionSet);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        SetId = set.Id;
        SetName = set.Name;
        SetText = setText ?? set.Text;
        QCount = set.Questions().Count;
        FirstQId = set.QuestionIds()[0];
        FirstQText = R<QuestionRepo>().GetById(FirstQId).Text; // this repo only needed to get Text of First Question to generate Link in Partial View!!
    }

    public static WelcomeBoxSingleSetModel GetWelcomeBoxSetSingleModel(int setId, string setText = null)
    {
        return new WelcomeBoxSingleSetModel(setId, setText);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public class QuestionSetImageSettings : IImageSettings
{
    private readonly int _questionSetId;
    public IEnumerable<int> SizesSquare { get { return new[] { 512, 206, 50, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 100, 500 }; } }
    public string BasePath { get { return "/Images/QuestionSets/"; } }
    
    public string BasePathAndId(){
        return HttpContext.Current.Server.MapPath(BasePath + _questionSetId);
    }

    public QuestionSetImageSettings(int questionSetId){
        _questionSetId = questionSetId;
    }

    public ImageUrl GetUrl_206px() { return GetUrl(206); }
    public ImageUrl GetUrl_500px() { return GetUrl(500); }

    private ImageUrl GetUrl(int width){
        return ImageUrl.Get(
            _questionSetId, 
            width, 
            BasePath,
            arg => "/Images/no-question-set-" + width + ".png"
        );
    }
}


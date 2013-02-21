using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public class QuestionImageSettings : IImageSettings
{
    private readonly int _questionId;

    public IEnumerable<int> SizesSquare { get { return new[] { 512, 128, 50, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 100, 500 }; } }

    public string BasePath { get { return "/Images/Questions/"; } }

    public QuestionImageSettings(int questionId){
        _questionId = questionId;
    }

    public string BasePathAndId(){
        return HttpContext.Current.Server.MapPath("/Images/Questions/" + _questionId);
    }

    public ImageUrl GetUrl_128px() { return GetUrl(128); }
    public ImageUrl GetUrl_500px() { return GetUrl(500); }

    private ImageUrl GetUrl(int width){
        return ImageUrl.Get(_questionId, width, BasePath, arg => "");
    }

    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public class QuestionImageSettings : IImageSettings
{
    public int Id { get; private set; }

    public IEnumerable<int> SizesSquare { get { return new[] { 512, 128, 50, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 100, 500 }; } }

    public string BasePath { get { return "/Images/Questions/"; } }

    public QuestionImageSettings(int questionId){
        Id = questionId;
    }

    public string ServerPathAndId(){
        return HttpContext.Current.Server.MapPath("/Images/Questions/" + Id);
    }

    public ImageUrl GetUrl_128px() { return GetUrl(128); }
    public ImageUrl GetUrl_500px() { return GetUrl(500); }

    private ImageUrl GetUrl(int width){
        return ImageUrl.Get(this, width, false, arg => "");
    }

    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

[Serializable]
public class CategoryTypeVolumeChapter : CategoryTypeBase<CategoryTypeVolumeChapter>
{
    public string Title;
    public string Subtitle;
    public string Author;
    public string TitleVolume;
    public string SubtitleVolume;
    public string Editor;
    public string ISBN;
    public string Publisher;
    public string PublicationCity;
    public string PublicationYear;
    public string PagesChapterFrom;
    public string PagesChapterTo;

    [JsonIgnore]
    public override CategoryType Type { get { return CategoryType.VolumeChapter; } }
}
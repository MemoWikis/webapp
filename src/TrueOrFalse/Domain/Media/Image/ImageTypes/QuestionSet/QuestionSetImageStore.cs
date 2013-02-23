using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;


public class QuestionSetImageStore : IRegisterAsInstancePerLifetime
{
    private readonly WikiMediaImageLoader _wikiMediaImageLoader;

    public QuestionSetImageStore(WikiMediaImageLoader wikiMediaImageLoader){
        _wikiMediaImageLoader = wikiMediaImageLoader;
    }

    public void RunWikimedia(string imageWikiFileName, int questionSetId)
    {
        using (var stream = _wikiMediaImageLoader.Run(imageWikiFileName)){
            StoreImages.Run(stream, new QuestionSetImageSettings(questionSetId));
        }
    }
}
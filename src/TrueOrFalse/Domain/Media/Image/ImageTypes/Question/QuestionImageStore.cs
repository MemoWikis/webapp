using System.Web;

public class QuestionImageStore
{
    public static void Run(HttpPostedFileBase imagefile, int questionId)
    {
        if (imagefile == null) 
            return;

        new StoreImages().Run(
                imagefile.InputStream,
                new QuestionImageSettings(questionId)
        );
    }
}
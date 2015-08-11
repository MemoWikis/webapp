using System.Web;

public class CategoryImageStore
{
    public static void Run(HttpPostedFileBase imagefile, int userId)
    {
        if (imagefile == null)
            return;

        SaveImageToFile.Run(
            imagefile.InputStream,
            new CategoryImageSettings(userId)
        );
    }
}


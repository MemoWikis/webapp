using System.Web;

public class UserImageStore
{
    public static void Run(HttpPostedFileBase imagefile, int userId)
    {
        if (imagefile == null)
            return;

        SaveImageToFile.Run(
            imagefile.InputStream,
            new UserImageSettings(userId)
        );
    }
}
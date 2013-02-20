using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public class ProfileImageStore
{
    public static void Run(HttpPostedFileBase imagefile, int userId)
    {
        if (imagefile == null)
            return;

        new StoreImages().Run(
                imagefile.InputStream,
                new ProfileImageSettings(userId)
        );
    }
}
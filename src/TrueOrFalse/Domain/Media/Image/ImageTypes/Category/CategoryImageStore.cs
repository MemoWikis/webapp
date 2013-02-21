using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public class CategoryImageStore
{
    public static void Run(HttpPostedFileBase imagefile, int userId)
    {
        if (imagefile == null)
            return;

        StoreImages.Run(
            imagefile.InputStream,
            new CategoryImageSettings(userId)
        );
    }
}


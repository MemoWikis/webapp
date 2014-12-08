using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalse;

public class TempHelperLicenseInfoFromDbOrSetNew
{
    public static void Run(ImageMetaData imageMetaData)
    {
        if(imageMetaData.AllRegisteredLicenses == null)
            imageMetaData.AllRegisteredLicenses = License.ToLicenseIdList(
                        LicenseParser.SortLicenses(LicenseParser.GetAllParsedLicenses(imageMetaData.Markup)));

        if(imageMetaData.MainLicenseInfo == null)
            LicenseParser.SetMainLicenseInfo(imageMetaData);
    }
}

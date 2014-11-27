using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ImageLicenseState
{
    NotSpecified = 0,
    LicenseIsApplicableForImage = 1,
    LicenseAuthorizedButInfoMissing = 2,
    LicenseIsNotAuthorized = 3,
}

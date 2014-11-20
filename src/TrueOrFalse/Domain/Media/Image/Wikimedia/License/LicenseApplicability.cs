using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum LicenseApplicability
{
    LicenseIsNotEvaluated = 0,
    LicenseAuthorizedAndAllRequirementsRecorded = 1,//Requirements should be recorded by chosing RequirementsType for License or by adding requirements manually when initializing new License
    LicenseIsNotApplicable = 2,
    LicenseIsConditionallyApplicable = 3,
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum LicenseApplicability
{
    LicenseIsNotEvaluated = 0,
    LicenseAuthorizedAndAllRequirementsRecorded = 1,
    LicenseIsNotApplicable = 2,
    LicenseIsConditionallyApplicable = 3,
}

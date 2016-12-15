public enum LicenseApplicability
{
    LicenseIsNotEvaluated = 0,
    /// <summary>
    /// Requirements should be recorded by chosing RequirementsType for License 
    /// or by adding requirements manually when initializing new License
    /// </summary>
    LicenseAuthorizedAndAllRequirementsRecorded = 1,
    LicenseIsNotApplicable = 2,
    LicenseIsConditionallyApplicable = 3,
}

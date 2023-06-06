using System.Collections.Generic;
using Seedworks.Lib.Persistence;

[Serializable]
public class ImageMetaDataSearchSpec : Pager
{
    public List<ImageLicenseState> LicenseStates = new List<ImageLicenseState>();

    public bool A;
    public bool B;
}
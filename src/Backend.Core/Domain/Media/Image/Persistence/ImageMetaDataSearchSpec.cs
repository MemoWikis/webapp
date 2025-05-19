[Serializable]
public class ImageMetaDataSearchSpec : Pager
{
    public List<ImageLicenseState> LicenseStates = new();

    public bool A;
    public bool B;
}
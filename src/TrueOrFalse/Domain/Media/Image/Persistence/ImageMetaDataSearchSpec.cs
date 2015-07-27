using System;
using Seedworks.Lib.Persistence;

[Serializable]
public class ImageMetaDataSearchSpec : SearchSpecificationBase<ImageMetaDataFilter, ImageMetaDataOrderBy>
{
    public ImageMetaDataSearchSpec()
    {
    }
}

[Serializable]
public class ImageMetaDataFilter : ConditionContainer
{
    public readonly ConditionString EmailAddress;
    public ImageMetaDataFilter(){
        EmailAddress = new ConditionString(this, "EmailAddress");
    }
}

[Serializable]
public class ImageMetaDataOrderBy : SpecOrderByBase
{
}
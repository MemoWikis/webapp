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
}

[Serializable]
public class ImageMetaDataOrderBy : SpecOrderByBase
{
}
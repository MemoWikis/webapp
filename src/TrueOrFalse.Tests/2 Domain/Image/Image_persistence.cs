using NUnit.Framework;

[Category(TestCategories.Programmer)]
public class Image_persistence : BaseTest
{
    [Test]
    public void Image_should_be_persisted()
    {
        var imageMetaData = new ImageMetaData();
        imageMetaData.Source = ImageSource.WikiMedia;
        imageMetaData.Type = ImageType.QuestionSet;
        imageMetaData.UserId = 2;

        Resolve<ImageMetaDataRepo>().Create(imageMetaData);
    }
}

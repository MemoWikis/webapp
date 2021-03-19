public class GetQuestionImageFrontendData
{
    public static ImageFrontendData Run(Question question)
    {
        var imageMetaDataRepo = Sl.Resolve<ImageMetaDataRepo>();
        var imageMetaData = imageMetaDataRepo.GetBy(question.Id, ImageType.Question);

        if (imageMetaData != null)
            return new ImageFrontendData(imageMetaData);

        foreach (var category in question.CategoriesIds)
        {
            imageMetaData = imageMetaDataRepo.GetBy(category.Id, ImageType.Category);

            if(imageMetaData != null)
                break;
        }

        return new ImageFrontendData(imageMetaData);
    }
}
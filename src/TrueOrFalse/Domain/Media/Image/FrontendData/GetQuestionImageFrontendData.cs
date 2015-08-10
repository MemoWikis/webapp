﻿public class GetQuestionImageFrontendData
{
    public static ImageFrontendData Run(Question question)
    {
        var imageMetaDataRepo = Sl.Resolve<ImageMetaDataRepository>();
        var imageMetaData = imageMetaDataRepo.GetBy(question.Id, ImageType.Question);

        if (imageMetaData != null)
            return new ImageFrontendData(imageMetaData);

        foreach (var category in question.Categories)
        {
            imageMetaData = imageMetaDataRepo.GetBy(category.Id, ImageType.Category);

            if(imageMetaData != null)
                break;
        }

        return new ImageFrontendData(imageMetaData);
    }
}
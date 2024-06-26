﻿using Microsoft.AspNetCore.Http;

public class GetQuestionImageFrontendData
{
    public static ImageFrontendData Run(
        QuestionCacheItem question,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo)
    {
        var imageMetaData = imageMetaDataReadingRepo.GetBy(question.Id, ImageType.Question);

        if (imageMetaData != null)
            return new ImageFrontendData(imageMetaData,
                httpContextAccessor,
                questionReadingRepo);
        ;

        foreach (var category in question.Categories)
        {
            imageMetaData = imageMetaDataReadingRepo.GetBy(category.Id, ImageType.Category);

            if (imageMetaData != null)
                break;
        }

        return new ImageFrontendData(imageMetaData,
            httpContextAccessor,
            questionReadingRepo);
        ;
    }
}
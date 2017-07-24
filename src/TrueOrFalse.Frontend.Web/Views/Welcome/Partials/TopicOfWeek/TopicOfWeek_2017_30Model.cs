using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;

public class TopicOfWeek_2017_30Model : BaseModel
{
    public Category Category;
    public int CategoryId;
    public string CategoryName;
    public string CategoryDescription;
    public ImageFrontendData ImageFrontendData;

    public int QuestionCount;
    public bool IsInWishknowledge;

    public IList<int> AdditionalSetsIds;
    public IList<int> AdditionalCategoriesIds;


    public TopicOfWeek_2017_30Model(int categoryId)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(264, ImageType.Category); //category: "Psychologie (Studium)" - for partial of Topic of the Week
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        AdditionalSetsIds = new List<int> { 123, 141, 135, 282 }; // and: , 148, 278
        AdditionalCategoriesIds = new List<int> { 271, 607, 618, 649 };

    }
}

using System;

public class SingleCategoryFullWidthModel : BaseModel
{
    public Category Category;
    public int CategoryId;
    public string CategoryType;
    public string Name;
    public string Description;
    public int AggregatedQuestionCount;
    public int AggregatedSetCount;
    public ImageFrontendData ImageFrontendData;
    public bool IsInWishknowledge;

    public SingleCategoryFullWidthModel(int categoryId, string name = null, string description = null)
    {
        Category = Sl.CategoryRepo.GetById(categoryId);
        CategoryId = Category.Id;
        if (Category== null)
            throw new Exception("Die angegebene Themen-ID verweist nicht auf ein existierendes Thema.");
        Name = name ?? Category.Name;
        Description = description ?? Category.Description;
        AggregatedQuestionCount = Category.CountQuestionsAggregated;
        AggregatedSetCount = Category.GetAggregatedSetsFromMemoryCache().Count;
        CategoryType = Category.Type.GetShortName();
        

        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(Category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        IsInWishknowledge = R<CategoryValuationRepo>().GetBy(CategoryId, _sessionUser.UserId)?.IsInWishKnowledge() ?? false;
    }

}

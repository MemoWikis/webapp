using System;
using System.Collections.Generic;
using System.Linq;

public class SegmentationModel : BaseContentModule
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;
    public List<Category> UnsortedCategoryList;
    public List<Segment> Segments;

    public SegmentationModel()
    {
    }

    public SegmentationModel(Category category)
    {
        Category = category;
        
        CategoryList = UserCache.IsFiltered ? UserEntityCache.GetChildren(category.Id, UserId) : Sl.CategoryRepo.GetChildren(category.Id).ToList();
        CategoryList = CategoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();

        var sortedCategories = new List<Category>();
        if (!IsLoggedIn)
        {
            foreach (var segment in Segments)
            {
                var categoriesToAdd = segment.CategoryList.Where(c => !sortedCategories.Any(s => s.Id == c.Id));
                foreach (var c in categoriesToAdd)
                    sortedCategories.Add(c);
            }

            UnsortedCategoryList = CategoryList.Where(c => !sortedCategories.Any(s => c.Id == s.Id)).ToList();
        } else
            UnsortedCategoryList = CategoryList;
    }

    public int GetTotalQuestionCount(Category category)
    {
        return category.GetAggregatedQuestionsFromMemoryCache().Count;
    }

    public ImageFrontendData GetCategoryImage(Category category)
    {
        var imageMetaData = Sl.ImageMetaDataRepo.GetBy(category.Id, ImageType.Category);
        return new ImageFrontendData(imageMetaData);
    }

    private List<Category> OrderByCategoryList(List<Category> firstCategories)
    {
        foreach (var category in firstCategories)
        {
            CategoryList.Remove(category);
        }

        firstCategories.AddRange(CategoryList);
        return firstCategories;
    }

    public static string ReturnKnowledgeStatus(List<string> list, int counter)
    {  
        return list.ElementAt(counter);
    }

    public int GetTotalTopicCount(Category category)
    {
        return EntityCache.GetChildren(category.Id).Count(c => c.Type == CategoryType.Standard && c.GetAggregatedQuestionIdsFromMemoryCache().Count > 0);

    }
}







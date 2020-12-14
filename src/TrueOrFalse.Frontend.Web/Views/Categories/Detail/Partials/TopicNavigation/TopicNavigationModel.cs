using System;
using System.Collections.Generic;
using System.Linq;

public class TopicNavigationModel : BaseContentModule
{
    public Category Category;

    public string Title;
    public string Text;

    public List<Category> CategoryList;

    public TopicNavigationModel()
    {
    }

    public TopicNavigationModel(Category category, TopicNavigationJson topicNavigation)
    {
        Category = category;

        var isLoadList = false;
        switch (topicNavigation.Load)
        {
            case null:
            case "All":
                CategoryList = UserCache.IsFiltered ? UserEntityCache.GetChildren(category.Id, UserId) : Sl.CategoryRepo.GetChildren(category.Id).ToList();
                break;
            default:
                var categoryIdList = topicNavigation.Load.Split(',').ToList().ConvertAll(int.Parse);
                CategoryList =  UserCache.IsFiltered ? EntityCache.GetCategories(categoryIdList).Where(c => c.IsInWishknowledge()).ToList() : EntityCache.GetCategories(categoryIdList).ToList();
                foreach (var category1 in CategoryList)
                {
                    Logg.r().Warning(category1.Id + "/Database Children");
                }
                isLoadList = true;
                break;
        }

        switch (topicNavigation.Order)
        {
            case null:
            case "QuestionAmount":
                if(topicNavigation.Load == null || topicNavigation.Load == "All")
                    CategoryList = CategoryList.OrderByDescending(c => c.GetAggregatedQuestionsFromMemoryCache().Count).ToList();
                break;

            case "Name":
                CategoryList = CategoryList.OrderBy(c => c.Name).ToList();
                break;

            default:
                if (isLoadList)
                {
                    throw new Exception("\"Load: \" und \"Order: \" können nicht gleichzeitig mit Category-Id-Listen als Parameter verwendet werden!");
                }

                if (!UserCache.IsFiltered)
                {
                    var firstCategories = EntityCache.GetCategories(
                            topicNavigation.Order.Split(',')
                                .ToList()
                                .ConvertAll(Int32.Parse))
                        .ToList();

                    CategoryList = OrderByCategoryList(firstCategories);
                }
               
                break;
        }

        CategoryList = CategoryList.Where(c => c.Type.GetCategoryTypeGroup() == CategoryTypeGroup.Standard).ToList();

        Title = topicNavigation.Title;
        Text = topicNavigation.Text;
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







using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CategoryModel : BaseModel
{
    public int Id;
    public string Name;
    public string Description;
    public string Type;

    public string CustomPageHtml;//Is set in controller because controller context is needed
    public IList<Set> FeaturedSets;

    public IList<Category> CategoriesParent;
    public IList<Category> CategoriesChildren;

    public IList<Set> Sets;
    public IList<Question> TopQuestions;
    public IList<Question> TopQuestionsWithReferences;
    public List<Question> TopQuestionsInSubCats = new List<Question>();
    public IList<Question> TopWishQuestions;
    public IList<Question> SingleQuestions;
    public IList<User> TopCreaters;


    public User Creator;
    public string CreatorName;
    public string CreationDate;
    public string CreationDateNiceText;

    public Category Category;

    public ImageFrontendData ImageFrontendData;

    public string WikiUrl;

    public bool IsOwnerOrAdmin;

    public int CountQuestions;
    public int CountReferences;
    public int CountWishQuestions;
    public int CountSets;
    public int CountCreators;

    public int CorrectnesProbability;
    public int AnswersTotal;

    private readonly QuestionRepo _questionRepo;
    private readonly CategoryRepository _categoryRepo;

    public CategoryModel(Category category)
    {
        _questionRepo = R<QuestionRepo>();
        _categoryRepo = R<CategoryRepository>();

        WikiUrl = category.WikipediaURL;
        Category = category;

        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
        Type = category.Type.GetShortName();

        FeaturedSets = category.FeaturedSets;

        IsOwnerOrAdmin = _sessionUser.IsLoggedInUserOrAdmin(category.Creator.Id);

        CategoriesParent = category.ParentCategories;
        CategoriesChildren = _categoryRepo.GetChildren(category.Id);

        CorrectnesProbability = category.CorrectnessProbability;
        AnswersTotal = category.CorrectnessProbabilityAnswerCount;

        var imageMetaData = Resolve<ImageMetaDataRepo>().GetBy(category.Id, ImageType.Category);
        ImageFrontendData = new ImageFrontendData(imageMetaData);

        var wishQuestions = _questionRepo.GetForCategoryAndInWishCount(category.Id, UserId, 5);

        CountQuestions = category.CountQuestions +
            R<QuestionGetCount>().Run(UserId, category.Id, new[] {QuestionVisibility.Owner, QuestionVisibility.OwnerAndFriends});

        CountReferences = ReferenceCount.Get(category.Id);

        if (category.Type != CategoryType.Standard)
            TopQuestionsWithReferences = Sl.R<ReferenceRepo>().GetQuestionsForCategory(category.Id);

        CountSets = category.CountSets;
        CountCreators = category.CountCreators;
        CountWishQuestions = wishQuestions.Total;

        TopQuestions = category.Type == CategoryType.Standard ? 
            _questionRepo.GetForCategory(category.Id, 5, UserId) : 
            _questionRepo.GetForReference(category.Id, 5, UserId);

        if (category.Type == CategoryType.Standard)
            TopQuestionsInSubCats = GetTopQuestionsInSubCats();

        TopWishQuestions = wishQuestions.Items;

        Sets = Resolve<SetRepo>().GetForCategory(category.Id);

        SingleQuestions = GetSingleQuestions();
    }

    private List<Question> GetSingleQuestions()
    {
        var result = new List<Question>();

        var a = Sl.R<QuestionRepo>().GetForCategory(Id);

        var b = Sets.SelectMany(s => s.QuestionsInSet).Select(x => x.Question);

        result = a.Except(b).ToList();

        return result;
    }

    private List<Question> GetTopQuestionsInSubCats()
    {
        var result = new List<Question>();
        
        foreach (var childCat in CategoriesChildren)
            if (FillResult(result, childCat))
                break;

        if (TopQuestionsInSubCats.Count < 3)
            foreach (var childCat in CategoriesChildren)
                foreach(var childOfChild in _categoryRepo.GetChildren(childCat.Id))
                    if (FillResult(result, childOfChild))
                        break;

        return result
            .Distinct(ProjectionEqualityComparer<Question>.Create(x => x.Id))
            .ToList();
    }

    private bool FillResult(List<Question> result, Category cat)
    {
        if (TopQuestionsInSubCats.Count > 15)
            return true;

        result.AddRange(_questionRepo.GetForCategory(cat.Id, 5, UserId));
        return false;
    }
}
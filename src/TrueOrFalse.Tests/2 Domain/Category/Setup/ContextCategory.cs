using System.Collections.Generic;
using System.Linq;
using SolrNet;

namespace TrueOrFalse.Tests
{
    public class ContextCategory
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ContextUser _contextUser = ContextUser.New();
        
        public List<Category> All = new List<Category>();

        public static ContextCategory New(bool addContextUser = true)
        {
            return new ContextCategory(addContextUser);
        }

        private ContextCategory(bool addContextUser = true)
        {
            _categoryRepository = Sl.R<CategoryRepository>();

            if(addContextUser)
                _contextUser.Add("Context Category" ).Persist();
        }

        public ContextCategory Add(int amount)
        {
            for (var i = 0; i < amount; i++)
                Add($"category name {0}");

            return this;
        }

        public ContextCategory Add(
            string categoryName, 
            CategoryType categoryType = CategoryType.Standard, 
            User creator = null, 
            Category parent = null)
        {
            Category category;
            if (_categoryRepository.Exists(categoryName))
            {  
                category = _categoryRepository.GetByName(categoryName).First();
            }
            else
            {
                category = new Category
                {
                    Name = categoryName,
                    Creator = creator ?? _contextUser.All.First(),
                    Type = categoryType,
                };
            }

            var categoryRelations = category.CategoryRelations.Count != 0 ? category.CategoryRelations : new List<CategoryRelation>();

            if (parent != null) // set parent
            {
                
                categoryRelations.Add(new CategoryRelation
                {
                    Category = category,
                    RelatedCategory = parent,
                    CategoryRelationType = CategoryRelationType.IsChildOf
                });

                category.CategoryRelations = categoryRelations;
            }

            if(!_categoryRepository.Exists(categoryName))
                All.Add(category);
            return this;
        }

        public ContextCategory AddToEntityCache(string categoryName, CategoryType categoryType = CategoryType.Standard, User creator = null, bool withId = false, int categoryId = 0)
        {
            var category = new Category();

            if (withId && categoryId == 0)
                category.Id = 0;
            else
                category.Id = categoryId; 

            category.Name = categoryName;
            category.Creator = creator == null ? _contextUser.All.FirstOrDefault() : creator ;
            category.Type = categoryType;

            var categoryCacheItem = CategoryCacheItem.ToCacheCategory(category);
            EntityCache.AddOrUpdate(categoryCacheItem);
            EntityCache.UpdateCategoryReferencesInQuestions(categoryCacheItem, category);

            All.Add(category);
            return this;
        }


        public ContextCategory QuestionCount(int questionCount)
        {
            All.Last().CountQuestions = questionCount;
            return this;
        }

        public ContextCategory Persist()
        { foreach(var cat in All)
                if(cat.Id <= 0) //if not allread created
                    _categoryRepository.Create(cat);

            return this;
        }

        public ContextCategory Update(Category category)
        {
            _categoryRepository.Update(category);

            _categoryRepository.Session.Flush();
            return this; 
        }

        public ContextCategory Update()
        {
            foreach (var cat in All)
                _categoryRepository.Update(cat);

            _categoryRepository.Session.Flush();

            return this;            
        }

        public ContextCategory Delete(Category category)
        {
            _categoryRepository.Delete(category);
            return this;
        }

        public ContextCategory AddRelationsToCategory(Category category, List<CategoryRelation> categoryRelations)
        {
            category.CategoryRelations = categoryRelations;
            _categoryRepository.Update(category);
            return this;
        }

        public User AddCaseThreeToCache(bool withWuwi = true)
        {
            //Add this Case: https://drive.google.com/file/d/1CEMMm1iIhfNKvuKng5oM6erR0bVDWHr6/view?usp=sharing


            var rootElement = Add("A").Persist().All.First();

            var firstChildren = 
                 Add("X", parent: rootElement)
                .Add("X1", parent: rootElement)
                .Add("X2", parent: rootElement)
                .Add("X3", parent: rootElement)
                .Persist().All;

            Add("X1", parent: firstChildren.ByName("X3")); 


                var secondChildren = Add("B", parent: rootElement)
                .Add("C", parent: firstChildren.ByName("X"))
                .Persist().All;

                Add("C", parent: firstChildren.ByName("X1")).Persist();
                Add("C", parent: firstChildren.ByName("X2")).Persist();


            var ThirdChildren = Add("H", parent: firstChildren.ByName("C"))
                .Add("G", parent: secondChildren.ByName("C"))
                .Add("F", parent: secondChildren.ByName("C"))
                .Add("E", parent: secondChildren.ByName("C"))
                .Add("D", parent: secondChildren.ByName("B"))
                .Persist()
                .All;

            Add("I", parent: secondChildren.ByName("C")).Persist();
            Add("I", parent: secondChildren.ByName("E")).Persist();
            Add("I", parent: secondChildren.ByName("G")).Persist();
               
            

            var user = ContextUser.New().Add("User").Persist().All[0];
            

            if (withWuwi)
            {
                // Add in WUWI
                CategoryInKnowledge.Pin(firstChildren.ByName("B").Id, user);
                CategoryInKnowledge.Pin(firstChildren.ByName("G").Id, user);
                CategoryInKnowledge.Pin(firstChildren.ByName("F").Id, user);
                CategoryInKnowledge.Pin(firstChildren.ByName("I").Id, user);
                CategoryInKnowledge.Pin(firstChildren.ByName("X").Id, user);
                CategoryInKnowledge.Pin(firstChildren.ByName("X3").Id, user);
            }
            Sl.SessionUser.Login(user);
            EntityCache.Init();
            UserEntityCache.Init(user.Id);
            return user; 
        }

        public void AddCaseTwoToCache()
        {
            //  this method display this case https://docs.google.com/drawings/d/1yoBx4OAUT3W2is9WpWczZ7Qb-lwvZeAGqDZYnP89wNk/
            var rootElement = Add("A").Persist().All.First();

            var firstChildren =
                Add("B", parent: rootElement)
                .Add("C", parent: rootElement)
                .Persist()
                .All;

            var secondChildren = 
                 Add("H", parent: firstChildren.ByName("C"))
                .Add("G", parent: firstChildren.ByName("C"))
                .Add("F", parent: firstChildren.ByName("C"))
                .Add("E", parent: firstChildren.ByName("C"))
                .Add("D", parent: firstChildren.ByName("B"))
                .Persist()
                .All;

            Add("I", parent: secondChildren.ByName("C"))
                .Persist();

            Add("I", parent: secondChildren.ByName("E"))
                .Persist();

            Add("I", parent: secondChildren.ByName("G"))
                .Persist();

            var user = ContextUser.New().Add("User").Persist().All[0];

            // Add in WUWI
            CategoryInKnowledge.Pin(firstChildren.ByName("B").Id, user);
            CategoryInKnowledge.Pin(firstChildren.ByName("G").Id, user);
            CategoryInKnowledge.Pin(firstChildren.ByName("E").Id, user);
            CategoryInKnowledge.Pin(firstChildren.ByName("I").Id, user);

            Sl.SessionUser.Login(user);
        }

        public static bool HasCorrectChild(CategoryCacheItem categoryCachedItem, string childName)
        {
            return categoryCachedItem.CachedData.ChildrenIds.Any(child => child == EntityCache.GetByName(childName).First().Id );
        }

        public static bool HasCorrectParent(CategoryCacheItem categoryCachedItem, string parentName)
        {
            return categoryCachedItem.CategoryRelations.Any(cr =>
                cr.RelatedCategoryId == EntityCache.GetByName(parentName).First().Id && cr.CategoryRelationType == CategoryRelationType.IsChildOf);
        }

        public static bool HasCorrectIncludetContent(CategoryCacheItem categoryCacheItem, string name, int userId)
        {
            return categoryCacheItem.CategoryRelations
                .Any(cr => cr.RelatedCategoryId == UserEntityCache.GetAllCategories(userId).ByName(name).Id &&
                           cr.CategoryRelationType == CategoryRelationType.IncludesContentOf); 
        }

        public static bool isIdAvailableInRelations(CategoryCacheItem categoryCacheItem, int deletedId)
        {
            return categoryCacheItem.CategoryRelations.Any(cr =>
                cr.RelatedCategoryId == deletedId || cr.CategoryId == deletedId); 
        }
    }
}

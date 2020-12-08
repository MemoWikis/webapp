using System.Collections.Generic;
using System.Linq;

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
            Category parent = null,
            bool IsInWishknowledge = false)
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

            if (parent != null)
            {
                var categoryRelations = category.CategoryRelations.Count != 0 ? category.CategoryRelations : new List<CategoryRelation>();
                categoryRelations.Add(new CategoryRelation
                {
                    Category = category,
                    RelatedCategory = parent,
                    CategoryRelationType = CategoryRelationType.IsChildCategoryOf
                });

                category.CategoryRelations = categoryRelations;
            }


            All.Add(category);
            return this;
        }

        public ContextCategory AddToEntityCache(string categoryName, CategoryType categoryType = CategoryType.Standard, User creator = null, bool withId = false)
        {
            var category = new Category();

            if (withId)
                category.Id = 0;

            category.Name = categoryName;
            category.Creator = creator == null ? _contextUser.All.FirstOrDefault() : creator ;
            category.Type = categoryType;

            EntityCache.AddOrUpdate(category);

            All.Add(category);
            return this;
        }

        public ContextCategory QuestionCount(int questionCount)
        {
            All.Last().CountQuestions = questionCount;
            return this;
        }

        public ContextCategory Persist()
        {
            foreach(var cat in All)
                if(cat.Id <= 0) //if not allread created
                    _categoryRepository.Create(cat);

            return this;
        }

        public ContextCategory Update()
        {
            foreach (var cat in All)
                _categoryRepository.Update(cat);

            _categoryRepository.Session.Flush();

            return this;            
        }

        public ContextCategory AddRelationsToCategory(Category category, List<CategoryRelation> categoryRelations)
        {
            category.CategoryRelations = categoryRelations;
            _categoryRepository.Update(category);
            return this;
        }
    }
}

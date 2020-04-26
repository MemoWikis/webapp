using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests
{
    public class ContextCategory
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ContextUser _contextUser = ContextUser.New();
        
        public List<Category> All = new List<Category>();

        public static ContextCategory New()
        {
            return new ContextCategory();
        }

        private ContextCategory()
        {
            _categoryRepository = Sl.R<CategoryRepository>();
           // _contextUser.Add("Context Category" ).Persist();
        }

        public ContextCategory Add(int amount)
        {
            for (var i = 0; i < amount; i++)
                Add($"category name {0}");

            return this;
        }

        public ContextCategory Add(string categoryName, CategoryType categoryType = CategoryType.Standard, User creator = null, bool withId = false)
        {
            Category category;
            if (_categoryRepository.Exists(categoryName))
            {  
                category = _categoryRepository.GetByName(categoryName).First();
            }
            else
            {
                category = new Category();
                if (withId)
                    category.Id = 0;
                
                    category.Name = categoryName;
                    category.Creator = creator;
                    category.Type = categoryType;

               if(!withId) 
                _categoryRepository.Create(category);
            }

            All.Add(category);
            return this;
        }

        public void AddToEntityCache(string categoryName, CategoryType categoryType = CategoryType.Standard, User creator = null, bool withId = false)
        {
            var category = new Category();

            if (withId)
                category.Id = 0;

            category.Name = categoryName;
            category.Creator = creator;
            category.Type = categoryType;

            EntityCache.AddOrUpdate(category);
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
    }
}

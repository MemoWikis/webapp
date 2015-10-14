using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests
{
    public class ContextCategory : IRegisterAsInstancePerLifetime
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ContextUser _contextUser = ContextUser.New();
        
        public List<Category> All = new List<Category>();

        public ContextCategory(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _contextUser.Add("Context Category" ).Persist();
        }

        public static ContextCategory New()
        {
            return BaseTest.Resolve<ContextCategory>();
        }

        public ContextCategory Add(string categoryName, CategoryType categoryType = CategoryType.Standard, User creator = null)
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
                    Type = categoryType
                };
                //_categoryRepository.Create(category); //Christof took this code line out, because otherwise [Test]Write_activity_category_set didn't work.
                //todo: ask robert if okay (see prev. line)
            }

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

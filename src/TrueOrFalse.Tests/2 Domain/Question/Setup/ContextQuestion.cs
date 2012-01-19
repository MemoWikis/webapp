using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Infrastructure;

namespace TrueOrFalse.Tests
{
    public class ContextQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepository;
        private readonly CategoryRepository _categoryRepository;
        
        public List<Question> Questions = new List<Question>();

        public static ContextQuestion New()
        {
            return BaseTest.Resolve<ContextQuestion>();
        }

        public ContextQuestion(QuestionRepository questionRepository, 
                               CategoryRepository categoryRepository)
        {
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
        }

        public ContextQuestion AddQuestion(string questionText, string solutionText)
        {
            var question = new Question();
            question.Text = questionText;
            question.Solution = solutionText;
            Questions.Add(question);
            return this;
        }

        public ContextQuestion AddCategory(string categoryName)
        {
            Questions.Last().Categories.Add(new Category(categoryName));
            return this;
        }

        public ContextQuestion Persist()
        {
            foreach (var question in Questions)
            {
                PersistNonExisitingCategories(question.Categories);
                _questionRepository.Create(question);
            }
            return this;
        }

        private void PersistNonExisitingCategories(IEnumerable<Category> categories)
        {
            foreach(var category in categories)
            {
                if(!_categoryRepository.Exists(category.Name))
                {
                    _categoryRepository.Create(category);
                }
                    
            }
        }
    }
}

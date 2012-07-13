using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Core;

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
            question.SolutionType = QuestionSolutionType.Exact;
            Questions.Add(question);
            return this;
        }

        public ContextQuestion AddCategory(string categoryName)
        {
            Category category;
            if (_categoryRepository.Exists(categoryName))
                category = _categoryRepository.GetByName(categoryName);
            else
            {
                category = new Category(categoryName);
                _categoryRepository.Create(category);
            }

            Questions.Last().Categories.Add(category);
            return this;
        }

        public ContextQuestion Persist()
        {
            foreach (var question in Questions)
                _questionRepository.Create(question);
            
            return this;
        }

    }
}

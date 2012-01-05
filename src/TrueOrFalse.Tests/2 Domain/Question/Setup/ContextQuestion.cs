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

        public ArrangeQuestion AddQuestion(string questionText)
        {
            var arrangeQuestion = new ArrangeQuestion(this);
            arrangeQuestion.Question.Text= questionText;
            Questions.Add(arrangeQuestion.Question);
            return arrangeQuestion;
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

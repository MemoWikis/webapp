﻿using System.Collections.Generic;
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
            _contextUser.Add("Context Category" ).Persist();
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
                _categoryRepository.Create(category);
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

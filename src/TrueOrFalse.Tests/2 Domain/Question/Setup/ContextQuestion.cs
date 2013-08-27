﻿using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests
{
    public class ContextQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly ContextUser _contextUser = ContextUser.New();
        private readonly ContextCategory _contextCategory = ContextCategory.New();

        private readonly QuestionRepository _questionRepository;
        

        public List<Question> All = new List<Question>();

        public static ContextQuestion New()
        {
            return BaseTest.Resolve<ContextQuestion>();
        }

        public ContextQuestion(QuestionRepository questionRepository)
        {
            _contextUser.Add("Some User").Persist();
            _contextUser.Add("Context Question").Persist();
            _questionRepository = questionRepository;
        }

        public ContextQuestion AddQuestion(string questionText, string solutionText)
        {
            var question = new Question();
            question.Text = questionText;
            question.Solution = solutionText;
            question.SolutionType = SolutionType.Text;
            question.SolutionMetadataJson = new SolutionMetadataText{IsCaseSensitive = true, IsExactInput = false}.Json;
            question.Creator = _contextUser.All.First();
            All.Add(question);
            return this;
        }

        public ContextQuestion AddCategory(string categoryName)
        {
            _contextCategory.Add(categoryName);
            All.Last().Categories.Add(_contextCategory.All.Last());
            return this;
        }

        public ContextQuestion Persist()
        {
            foreach (var question in All)
                _questionRepository.Create(question);

            return this;
        }

    }
}

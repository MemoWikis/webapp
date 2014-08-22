using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests
{
    public class ContextQuestion : IRegisterAsInstancePerLifetime
    {
        private readonly ContextUser _contextUser = ContextUser.New();
        private readonly ContextCategory _contextCategory = ContextCategory.New();

        private readonly QuestionRepository _questionRepository;

        public List<Question> All = new List<Question>();
        public User Creator { get { return _contextUser.All[0]; }}

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

        public ContextQuestion AddQuestion(string questionText = "defaultText", string solutionText = "defaultSolution")
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

        public ContextQuestion TotalQualityEntries(int totalQualityEntries){ All.Last().TotalQualityEntries = totalQualityEntries; return this;}
        public ContextQuestion TotalQualityAvg(int totalQualityAvg) { All.Last().TotalQualityAvg = totalQualityAvg; return this; }
        public ContextQuestion TotalValuationAvg(int totalValuationAvg) { All.Last().TotalRelevancePersonalAvg = totalValuationAvg; return this; }

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

            _questionRepository.Flush();

            return this;
        }

    }
}

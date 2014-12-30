using FluentNHibernate.Mapping;

namespace TrueOrFalse
{
    public class QuestionValuationMap : ClassMap<QuestionValuation>
    {
        public QuestionValuationMap()
        {
            Id(x => x.Id);
            References(x => x.User).Column("UserId");
            References(x => x.Question).Column("QuestionId");

            Map(x => x.Quality);
            Map(x => x.RelevancePersonal);
            Map(x => x.RelevanceForAll);

            Map(x => x.CorrectnessProbability);
            Map(x => x.KnowledgeStatus);

            Map(x => x.DateCreated);
        }
    }
}
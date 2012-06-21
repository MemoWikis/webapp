using FluentNHibernate.Mapping;

namespace TrueOrFalse.Core
{
    public class QuestionValuationMap : ClassMap<QuestionValuation>
    {
        public QuestionValuationMap()
        {
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.QuestionId);

            Map(x => x.Quality);
            Map(x => x.RelevancePersonal);
            Map(x => x.RelevanceForAll);

            Map(x => x.DateCreated);
        }
    }
}

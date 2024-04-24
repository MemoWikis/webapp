using FluentNHibernate.Mapping;

public class CommentMap : ClassMap<Comment>
{
    public CommentMap ()
    {
        Id(x => x.Id);

        Map(x => x.Type).CustomType<CommentType>();
        Map(x => x.TypeId);

        References(x => x.AnswerTo).Column("AnswerTo").Cascade.None();
        HasMany(x => x.Answers).Cascade.None().KeyColumn("AnswerTo");

        Map(x => x.ShouldImprove);
        Map(x => x.ShouldRemove);
        Map(x => x.ShouldKeys);

        Map(x => x.IsSettled);
          
        References(x => x.Creator);
        Map(x => x.Title); 
        Map(x => x.Text);

        Map(x => x.DateCreated);
        Map(x => x.DateModified);
    }
}

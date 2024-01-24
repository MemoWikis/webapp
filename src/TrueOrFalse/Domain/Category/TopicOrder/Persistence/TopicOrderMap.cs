using FluentNHibernate.Mapping;

//public class TopicOrderMap : ClassMap<TopicOrderNode>
//{
//    public TopicOrderMap()
//    {
//        Table("topicorder");
//        Id(x => x.Id);

//        References(x => x.TopicId).Column("TopicId").Cascade.None();
//        References(x => x.ParentId).Column("ParentId").Cascade.None();
//        References(x => x.PreviousId).Column("PreviousId").Cascade.None();
//        References(x => x.NextId).Column("NextId").Cascade.None();
//    }
//}
using FluentNHibernate.Mapping;

public class JobQueueMap : ClassMap<JobQueue>
{
    public JobQueueMap()
    {
        Id(x => x.Id);
        Map(x => x.JobQueueType).CustomType<JobQueueType>();
        Map(x => x.JobContent);
        Map(x => x.Priority);
    }
}
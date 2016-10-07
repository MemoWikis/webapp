using FluentNHibernate.Mapping;

public class RunningJobMap : ClassMap<RunningJob>
{
    public RunningJobMap()
    {
        Id(x => x.Id);
        Map(x => x.StartAt);
        Map(x => x.Name);
    }
}
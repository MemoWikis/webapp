public interface IMaintenanceOperation
{
    Task Run(string jobId);
    string OperationName { get; }
}
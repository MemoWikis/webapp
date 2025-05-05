using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public sealed class MissingHttpVerbLogger(IApiDescriptionGroupCollectionProvider _api, ILogger<MissingHttpVerbLogger> _log)
    : IHostedService
{
    public Task StartAsync(CancellationToken ct)
    {
        var withoutVerb = _api.ApiDescriptionGroups.Items
            .SelectMany(g => g.Items)
            .Where(d => string.IsNullOrEmpty(d.HttpMethod))
            .Select(d => $"{d.ActionDescriptor.DisplayName}  ({d.RelativePath})");

        foreach (var a in withoutVerb) 
            _log.LogWarning("No HTTP verb → {Action}", a);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
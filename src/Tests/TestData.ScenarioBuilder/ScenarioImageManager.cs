public static class ScenarioImageManager
{
    /// <summary>Creates a new scenation docker Image and pushes it to registry.</summary>
    public static async Task<string> BuildAndPushAsync(
        ScenarioConfiguration cfg,
        string? tag = null,
        bool saveLocalTar = false,
        CancellationToken ct = default)
    {
        tag ??= DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var fullName = $"{ScenarioImageConstants.BaseName}:{tag}";

        // 1. Harness setup
        await using var harness = await TestHarness.CreateAsync(enablePerfLogging: true);
        await harness.InitAsync(keepData: false);

        // 2. Szenario create
        var perf = new PerformanceLogger(enabled: true);
        var builder = new ScenarioBuilder(harness, cfg, perf);
        await builder.BuildAsync();

        // 3. Container → Image
        var docker = new DockerUtilities();
        await docker.PersistToDockerImageAsync(fullName, saveToFile: saveLocalTar);
        await docker.PushDockerImageToRepositoryAsync(fullName);

        return fullName; 
    }
}
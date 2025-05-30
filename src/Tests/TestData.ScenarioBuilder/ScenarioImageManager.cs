public static class ScenarioImageManager
{
    /// <summary>Creates a new scenario docker Image and optionally pushes it to registry.</summary>
    private static async Task<string> BuildImageAsync(string? tag, bool saveLocalTar, string? registry)
    {
        tag ??= DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var fullName = $"{ScenarioImageConstants.BaseName}:{tag}";
        var perfLogger = new PerformanceLogger(enabled: true);

        // Container → Image
        var docker = new DockerUtilities();
        await docker.PersistToDockerImageAsync(fullName, saveToFile: saveLocalTar);

        // Push if registry specified
        if (!string.IsNullOrEmpty(registry))
        {
            await docker.PushDockerImageToRepositoryAsync(fullName, registry);
            perfLogger.Log($"Image {fullName} built and pushed to {registry} successfully.");
        }
        else
        {
            perfLogger.Log($"Image {fullName} built successfully (local only).");
        }

        return fullName;
    }

    /// <summary>Creates a new scenario docker Image locally (no push).</summary>
    public static Task<string> BuildAndPushAsync(string? tag = null, bool saveLocalTar = false) 
        => BuildImageAsync(tag, saveLocalTar, registry: null);

    /// <summary>Creates a new scenario docker Image and pushes it to GitHub registry.</summary>
    public static Task<string> BuildAndPushToGitHubAsync(string githubUsername, string? tag = null, bool saveLocalTar = false) 
        => BuildImageAsync(tag, saveLocalTar, registry: $"ghcr.io/{githubUsername}");
}
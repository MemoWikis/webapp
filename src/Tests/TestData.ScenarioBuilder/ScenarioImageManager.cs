public static class ScenarioImageManager
{
    /// <summary>Creates a new scenario docker Image and pushes it to registry.</summary>
    public static async Task<string> BuildAndPushAsync(string? tag = null, bool saveLocalTar = false)
    {
        tag ??= DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var fullName = $"{ScenarioImageConstants.BaseName}:{tag}";


        var perfLogger = new PerformanceLogger(enabled: true);

        // 3. Container → Image
        var docker = new DockerUtilities();
        await docker.PersistToDockerImageAsync(fullName, saveToFile: saveLocalTar);
        await docker.PushDockerImageToRepositoryAsync(fullName);

        perfLogger.Log($"Image {fullName} built and pushed successfully.");

        return fullName; 
    }
}
internal class DockerUtilities
{
    /// <summary>
    ///     Creates a Docker image of the current database state that can be saved to a central repository
    /// </summary>
    /// <param name="imageName">Name to give the Docker image</param>
    /// <param name="saveToFile">Whether to save the image to a local file</param>
    /// <returns>Path to the saved Docker image</returns>
    public async Task<string> PersistToDockerImageAsync(string imageName = "memowikis-test-scenario", bool saveToFile = true)
    {
        // Get the Docker container ID for the MySQL container
        string containerId = await GetMySqlContainerIdAsync();
        if (string.IsNullOrEmpty(containerId))
        {
            throw new InvalidOperationException("MySQL Docker container not found.");
        }

        // Create a Docker image from the running container
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string fullImageName = $"{imageName}:{timestamp}";
        await CommitContainerToImageAsync(containerId, fullImageName);

        // Optionally save image to file for later use
        string outputFile = string.Empty;
        if (saveToFile)
        {
            outputFile = $"{imageName}-{timestamp}.tar";
            await SaveDockerImageToFileAsync(fullImageName, outputFile);
        }

        return fullImageName;
    }

    /// <summary>
    ///     Pushes a Docker image to a central repository
    /// </summary>
    /// <param name="imageName">Name of the Docker image to push</param>
    /// <param name="repository">Repository URL (default: Docker Hub)</param>
    /// <returns>True if the push was successful</returns>
    public async Task<bool> PushDockerImageToRepositoryAsync(string imageName, string repository = "")
    {
        // Tag the image with the repository if provided
        if (!string.IsNullOrEmpty(repository))
        {
            var tagStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"tag {imageName} {repository}/{imageName}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var tagProcess = System.Diagnostics.Process.Start(tagStartInfo);
            if (tagProcess == null)
                return false;

            await tagProcess.WaitForExitAsync();
            if (tagProcess.ExitCode != 0)
                return false;

            imageName = $"{repository}/{imageName}";
        }

        // Push the image to the repository
        var pushStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"push {imageName}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var pushProcess = System.Diagnostics.Process.Start(pushStartInfo);
        if (pushProcess == null)
            return false;

        await pushProcess.WaitForExitAsync();
        return pushProcess.ExitCode == 0;
    }

    /// <summary>
    ///     Loads a Docker image from a repository or local file
    /// </summary>
    /// <param name="imageNameOrPath">Name of the Docker image or path to the saved image file</param>
    /// <returns>True if the image was successfully loaded</returns>
    public static async Task<bool> LoadDockerImageAsync(string imageNameOrPath)
    {
        string arguments;

        if (imageNameOrPath.EndsWith(".tar"))
        {
            // Load image from file
            arguments = $"load -i {imageNameOrPath}";
        }
        else
        {
            // Pull image from repository
            arguments = $"pull {imageNameOrPath}";
        }

        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            return false;

        await process.WaitForExitAsync();
        return process.ExitCode == 0;
    }

    private async Task<string> GetMySqlContainerIdAsync()
    {
        // Use Docker CLI to find the MySQL container for our tests
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "ps --filter \"name=mysql\" --format \"{{.ID}}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null) return string.Empty;

        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        return output.Trim();
    }

    private async Task CommitContainerToImageAsync(string containerId, string imageName)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"commit {containerId} {imageName}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            throw new InvalidOperationException("Failed to start Docker commit process.");

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"Docker commit failed with exit code {process.ExitCode}.");
    }

    private async Task SaveDockerImageToFileAsync(string imageName, string outputFile)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"save -o {outputFile} {imageName}",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            throw new InvalidOperationException("Failed to start Docker save process.");

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"Docker save failed with exit code {process.ExitCode}.");
    }
}
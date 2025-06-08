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
            throw new InvalidOperationException("MySQL Docker container not found. Make sure a container with 'mysql' in its name is running.");
        }

        // Check if imageName already contains a tag (has a colon)
        string fullImageName;
        string baseImageName;
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        if (imageName.Contains(':'))
        {
            // Image name already has a tag, use it as is
            fullImageName = imageName;
            baseImageName = imageName.Replace(':', '-');
        }
        else
        {
            // No tag present, add timestamp as tag
            fullImageName = $"{imageName}:{timestamp}";
            baseImageName = imageName;
        }

        await CommitContainerToImageAsync(containerId, fullImageName);

        // Optionally save image to file for later use
        string outputFile = string.Empty;
        if (saveToFile)
        {
            outputFile = $"{baseImageName}-{timestamp}.tar";
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
        string errorOutput;
        if (!string.IsNullOrEmpty(repository))
        {
            var tagStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "docker",
                Arguments = $"tag {imageName} {repository}/{imageName}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var tagProcess = System.Diagnostics.Process.Start(tagStartInfo);
            if (tagProcess == null)
                return false;

            errorOutput = await tagProcess.StandardError.ReadToEndAsync();
            await tagProcess.WaitForExitAsync();
            if (tagProcess.ExitCode != 0)
            {
                throw new InvalidOperationException($"Docker tag failed: {errorOutput}");
            }

            imageName = $"{repository}/{imageName}";
        }

        // Push the image to the repository
        var pushStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"push {imageName}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var pushProcess = System.Diagnostics.Process.Start(pushStartInfo);
        if (pushProcess == null)
            return false;

        string standardOutput = await pushProcess.StandardOutput.ReadToEndAsync();
        errorOutput = await pushProcess.StandardError.ReadToEndAsync();
        await pushProcess.WaitForExitAsync();

        if (pushProcess.ExitCode != 0)
        {
            throw new InvalidOperationException($"Docker push failed: {errorOutput}");
        }

        return true;
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
            // First, check if the image exists locally
            if (await ImageExistsLocallyAsync(imageNameOrPath))
            {
                // Image exists locally, no need to pull
                return true;
            }

            // Pull image from repository
            arguments = $"pull {imageNameOrPath}";
        }

        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            return false;

        string errorOutput = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Docker load/pull failed: {errorOutput}");
        }

        return true;
    }

    /// <summary>
    /// Checks if a Docker image exists locally
    /// </summary>
    /// <param name="imageName">Name of the Docker image to check</param>
    /// <returns>True if the image exists locally</returns>
    private static async Task<bool> ImageExistsLocallyAsync(string imageName)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"images {imageName} --quiet",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            return false;

        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        // If docker images returns output and no error, the image exists
        return process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output.Trim());
    }

    private async Task<string> GetMySqlContainerIdAsync()
    {
        // First, let's check all running containers to help with debugging
        var listStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "ps --format \"table {{.ID}}\\t{{.Names}}\\t{{.Status}}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var listProcess = System.Diagnostics.Process.Start(listStartInfo);
        if (listProcess != null)
        {
            string listOutput = await listProcess.StandardOutput.ReadToEndAsync();
            await listProcess.WaitForExitAsync();
            System.Diagnostics.Debug.WriteLine($"Running containers:\n{listOutput}");
        }

        // Use Docker CLI to find the MySQL container for our tests
        // Using -n 1 to get only the first container
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "ps --filter \"name=mem-mysql\" --format \"{{.ID}}\" -n 1",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null) return string.Empty;

        string output = await process.StandardOutput.ReadToEndAsync();
        string errorOutput = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (!string.IsNullOrEmpty(errorOutput))
        {
            System.Diagnostics.Debug.WriteLine($"Docker ps error: {errorOutput}");
        }

        // Take only the first line and trim it
        string containerId = output.Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim() ?? string.Empty;

        // If no container found with mem-mysql, try with just mysql
        if (string.IsNullOrEmpty(containerId))
        {
            var fallbackStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "ps --filter \"name=mysql\" --format \"{{.ID}}\" -n 1",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var fallbackProcess = System.Diagnostics.Process.Start(fallbackStartInfo);
            if (fallbackProcess != null)
            {
                output = await fallbackProcess.StandardOutput.ReadToEndAsync();
                await fallbackProcess.WaitForExitAsync();
                containerId = output.Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim() ?? string.Empty;
            }
        }

        System.Diagnostics.Debug.WriteLine($"Selected MySQL container ID: '{containerId}'");

        return containerId;
    }

    private async Task CommitContainerToImageAsync(string containerId, string imageName)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"commit {containerId} {imageName}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            throw new InvalidOperationException("Failed to start Docker commit process.");

        string standardOutput = await process.StandardOutput.ReadToEndAsync();
        string errorOutput = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Docker commit failed with exit code {process.ExitCode}. " +
                                                $"Container ID: '{containerId}', Image Name: '{imageName}'. " +
                                                $"Error: {errorOutput}. " +
                                                $"Output: {standardOutput}");
        }
    }

    private async Task SaveDockerImageToFileAsync(string imageName, string outputFile)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"save -o {outputFile} {imageName}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
            throw new InvalidOperationException("Failed to start Docker save process.");

        string errorOutput = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Docker save failed with exit code {process.ExitCode}. " +
                                                $"Error: {errorOutput}");
        }
    }
}
using System.Diagnostics;

internal class DockerUtilities
{
    /// <summary>
    /// Creates a standardized ProcessStartInfo for Docker commands
    /// </summary>
    /// <param name="arguments">Docker command arguments</param>
    /// <returns>Configured ProcessStartInfo</returns>
    private static ProcessStartInfo CreateDockerProcessStartInfo(string arguments)
    {
        return new ProcessStartInfo
        {
            FileName = "docker",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
    }

    /// <summary>
    /// Executes a Docker command and returns the result
    /// </summary>
    /// <param name="arguments">Docker command arguments</param>
    /// <param name="throwOnError">Whether to throw an exception on non-zero exit code</param>
    /// <returns>Docker command execution result</returns>
    private static async Task<DockerCommandResult> ExecuteDockerCommandAsync(string arguments)
    {
        var processStartInfo = CreateDockerProcessStartInfo(arguments);

        using var process = Process.Start(processStartInfo);
        if (process == null)
        {
            throw new InvalidOperationException($"Failed to start Docker process with arguments: {arguments}");
        }

        var standardOutput = await process.StandardOutput.ReadToEndAsync();
        var errorOutput = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        var result = new DockerCommandResult
        {
            ExitCode = process.ExitCode,
            StandardOutput = standardOutput,
            ErrorOutput = errorOutput,
            IsSuccess = process.ExitCode == 0
        };

        if (!result.IsSuccess)
        {
            throw new InvalidOperationException($"Docker command failed with exit code {result.ExitCode}. " +
                                                $"Arguments: {arguments}. Error: {result.ErrorOutput}");
        }

        return result;
    }

    /// <summary>
    /// Executes a MySQL command inside a Docker container
    /// </summary>
    /// <param name="containerIdentifier">Container ID or name</param>
    /// <param name="mysqlCommand">MySQL command to execute</param>
    /// <param name="throwOnError">Whether to throw an exception on non-zero exit code</param>
    /// <returns>Command execution result</returns>
    private static async Task<DockerCommandResult> ExecuteMySqlCommandAsync(string containerIdentifier, string mysqlCommand)
    {
        var arguments = $"exec {containerIdentifier} mysql -u root -p{TestConstants.MySqlPassword} -e \"{mysqlCommand}\"";
        return await ExecuteDockerCommandAsync(arguments);
    }

    /// <summary>
    /// Represents the result of a Docker command execution
    /// </summary>
    private record DockerCommandResult
    {
        public int ExitCode { get; init; }
        public string StandardOutput { get; init; } = string.Empty;
        public string ErrorOutput { get; init; } = string.Empty;
        public bool IsSuccess { get; init; }
    }

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
        if (!string.IsNullOrEmpty(repository))
        {
            var taggedImageName = $"{repository}/{imageName}";
            await ExecuteDockerCommandAsync($"tag {imageName} {taggedImageName}");
            imageName = taggedImageName;
        }

        // Push the image to the repository
        await ExecuteDockerCommandAsync($"push {imageName}");
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

        await ExecuteDockerCommandAsync(arguments);
        return true;
    }

    /// <summary>
    /// Checks if a Docker image exists locally
    /// </summary>
    /// <param name="imageName">Name of the Docker image to check</param>
    /// <returns>True if the image exists locally</returns>
    private static async Task<bool> ImageExistsLocallyAsync(string imageName)
    {
        var result = await ExecuteDockerCommandAsync($"images {imageName} --quiet");

        // If docker images returns output and no error, the image exists
        return result.IsSuccess && !string.IsNullOrWhiteSpace(result.StandardOutput.Trim());
    }

    private async Task<string> GetMySqlContainerIdAsync()
    {
        // First, let's check all running containers to help with debugging
        var listResult = await ExecuteDockerCommandAsync("ps --format \"table {{.ID}}\\t{{.Names}}\\t{{.Status}}\"");
        if (listResult.IsSuccess)
        {
            Debug.WriteLine($"Running containers:\n{listResult.StandardOutput}");
        }

        // Use Docker CLI to find the MySQL container for our tests
        // Using -n 1 to get only the first container
        var result = await ExecuteDockerCommandAsync("ps --filter \"name=memowikis-mysql\" --format \"{{.ID}}\" -n 1");

        if (!string.IsNullOrEmpty(result.ErrorOutput))
        {
            Debug.WriteLine($"Docker ps error: {result.ErrorOutput}");
        }

        // Take only the first line and trim it
        string containerId = result.StandardOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim() ?? string.Empty;

        // If no container found with memowikis-mysql, try with just mysql as fallback
        if (string.IsNullOrEmpty(containerId))
        {
            var fallbackResult = await ExecuteDockerCommandAsync("ps --filter \"name=mysql\" --format \"{{.ID}}\" -n 1");
            if (fallbackResult.IsSuccess)
            {
                containerId = fallbackResult.StandardOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()?.Trim() ?? string.Empty;
            }
        }

        Debug.WriteLine($"Selected MySQL container ID: '{containerId}'");

        return containerId;
    }

    private async Task CommitContainerToImageAsync(
        string containerIdentifier,
        string targetImageName)
    {
        // Force MySQL to flush all data to disk before committing
        await FlushMySqlDataAsync(containerIdentifier);

        // All labels that should be cleared (value will be "")
        string[] labelsToClear =
        [
            "org.testcontainers",
            "org.testcontainers.lang",
            "org.testcontainers.resource-reaper-session",
            "org.testcontainers.session-id",
            "org.testcontainers.version",
            "desktop.docker.io/ports.scheme",
            "desktop.docker.io/ports/3306/tcp"
        ];

        string labelArgs = string.Join(' ',
            labelsToClear.Select(labelKey =>
                $"--change \"LABEL {labelKey}=\""));

        await ExecuteDockerCommandAsync($"commit {labelArgs} {containerIdentifier} {targetImageName}");
    }

    /// <summary>
    /// Flushes MySQL data to disk to ensure persistence when creating container images
    /// </summary>
    private async Task FlushMySqlDataAsync(string containerIdentifier)
    {
        try
        {
            // Execute MySQL FLUSH commands to ensure all data is written to disk
            var flushCommands = new[]
            {
                "FLUSH TABLES;",           // Flush all table data
                "FLUSH LOGS;",             // Flush log files
                "FLUSH BINARY LOGS;",      // Flush binary logs
                "SET GLOBAL innodb_fast_shutdown = 0;", // Clean shutdown for InnoDB
                "FLUSH ENGINE LOGS;"       // Flush storage engine logs
            };

            foreach (var command in flushCommands)
            {
                var result = await ExecuteMySqlCommandAsync(containerIdentifier, command);

                // Log any issues but don't fail the entire process
                if (!result.IsSuccess)
                {
                    Debug.WriteLine($"MySQL flush command '{command}' warning: {result.ErrorOutput}");
                }
            }

            // Give MySQL a moment to complete the flush operations
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Warning: Failed to flush MySQL data: {ex.Message}");
            throw;
        }
    }

    private async Task SaveDockerImageToFileAsync(string imageName, string outputFile)
    {
        await ExecuteDockerCommandAsync($"save -o {outputFile} {imageName}");
    }

    /// <summary>
    /// Exports the test database (mysqldump) from the running container to a .sql file on the host.
    /// </summary>
    public async Task ExportMySqlDumpAsync(string hostFilePath)
    {
        string containerIdentifier = await GetMySqlContainerIdAsync();
        string containerTempPath = "/tmp/testdump.sql";

        // Flush MySQL data to ensure all data is written to disk before dumping
        await FlushMySqlDataAsync(containerIdentifier);

        // Create dump inside the container using root user (test user lacks RELOAD/FLUSH_TABLES privileges)
        await ExecuteDockerCommandAsync(
            $"exec {containerIdentifier} sh -c \"mysqldump -u root -p{TestConstants.MySqlPassword} " +
            $"--databases {TestConstants.TestDbName} --routines --events --single-transaction --quick --flush-logs > {containerTempPath}\"");

        // Copy dump from container to host
        await ExecuteDockerCommandAsync($"cp {containerIdentifier}:{containerTempPath} {hostFilePath}");
    }

    /// <summary>
    /// Imports a .sql file located on the host into the running MySQL container.
    /// </summary>
    public async Task ImportMySqlDumpAsync(string hostFilePath)
    {
        string containerIdentifier = await GetMySqlContainerIdAsync();
        string containerTempPath = "/tmp/testdump.sql";

        // Copy file into container
        await ExecuteDockerCommandAsync($"cp {hostFilePath} {containerIdentifier}:{containerTempPath}");

        // Import into MySQL using root user (for consistency with export)
        await ExecuteDockerCommandAsync(
            $"exec {containerIdentifier} sh -c \"mysql -u root -p{TestConstants.MySqlPassword} < {containerTempPath}\"");
    }
}
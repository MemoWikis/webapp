using NUnit.Framework;
using System;
using System.Threading.Tasks;

[TestFixture]
public class DockerDiagnosticsTests
{
    private DockerUtilities _dockerUtilities;

    [SetUp]
    public void Setup()
    {
        _dockerUtilities = new DockerUtilities();
    }

    [Test]
    public async Task Check_Docker_Environment()
    {
        // Check if Docker is running
        var dockerVersionStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "version",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var versionProcess = System.Diagnostics.Process.Start(dockerVersionStartInfo);
        Assert.That(versionProcess, Is.Not.Null, "Could not start docker process");

        string versionOutput = await versionProcess.StandardOutput.ReadToEndAsync();
        await versionProcess.WaitForExitAsync();

        Console.WriteLine("Docker Version:");
        Console.WriteLine(versionOutput);
        Assert.That(versionProcess.ExitCode, Is.EqualTo(0), "Docker is not running or not installed");

        // List all running containers
        var listStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "ps --format \"table {{.ID}}\\t{{.Names}}\\t{{.Image}}\\t{{.Status}}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var listProcess = System.Diagnostics.Process.Start(listStartInfo);
        Assert.That(listProcess, Is.Not.Null);

        string listOutput = await listProcess.StandardOutput.ReadToEndAsync();
        await listProcess.WaitForExitAsync();

        Console.WriteLine("\nRunning Containers:");
        Console.WriteLine(listOutput);

        // Check specifically for MySQL containers
        var mysqlStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "ps --filter \"name=mysql\" --format \"{{.ID}} {{.Names}} {{.Status}}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var mysqlProcess = System.Diagnostics.Process.Start(mysqlStartInfo);
        Assert.That(mysqlProcess, Is.Not.Null);

        string mysqlOutput = await mysqlProcess.StandardOutput.ReadToEndAsync();
        string mysqlError = await mysqlProcess.StandardError.ReadToEndAsync();
        await mysqlProcess.WaitForExitAsync();

        Console.WriteLine("\nMySQL Containers:");
        Console.WriteLine(mysqlOutput);
        if (!string.IsNullOrEmpty(mysqlError))
        {
            Console.WriteLine("Error output:");
            Console.WriteLine(mysqlError);
        }

        Assert.That(mysqlOutput, Is.Not.Empty, "No MySQL container found. Make sure a container with 'mysql' in its name is running.");
    }

    [Test]
    public async Task Test_Container_Commit_With_Simple_Image()
    {
        // Get container ID
        var containerIdStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = "ps --filter \"name=mysql\" --format \"{{.ID}}\" -n 1",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var containerIdProcess = System.Diagnostics.Process.Start(containerIdStartInfo);
        Assert.That(containerIdProcess, Is.Not.Null);

        string containerId = await containerIdProcess.StandardOutput.ReadToEndAsync();
        await containerIdProcess.WaitForExitAsync();
        containerId = containerId.Trim();

        Assert.That(containerId, Is.Not.Empty, "No MySQL container found");
        Console.WriteLine($"Container ID: {containerId}");

        // Try a simple commit with basic image name
        string testImageName = "test-mysql-commit";
        var commitStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"commit {containerId} {testImageName}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var commitProcess = System.Diagnostics.Process.Start(commitStartInfo);
        Assert.That(commitProcess, Is.Not.Null);

        string commitOutput = await commitProcess.StandardOutput.ReadToEndAsync();
        string commitError = await commitProcess.StandardError.ReadToEndAsync();
        await commitProcess.WaitForExitAsync();

        Console.WriteLine($"Commit Output: {commitOutput}");
        if (!string.IsNullOrEmpty(commitError))
        {
            Console.WriteLine($"Commit Error: {commitError}");
        }

        Assert.That(commitProcess.ExitCode, Is.EqualTo(0),
            $"Docker commit failed. Error: {commitError}");

        // Clean up - remove the test image
        var removeStartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "docker",
            Arguments = $"rmi {testImageName}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var removeProcess = System.Diagnostics.Process.Start(removeStartInfo);
        if (removeProcess != null)
        {
            await removeProcess.WaitForExitAsync();
        }
    }
}
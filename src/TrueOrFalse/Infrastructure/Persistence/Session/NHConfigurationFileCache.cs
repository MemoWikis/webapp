using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using NHibernate.Cfg;
using Seedworks.Web.State;

//see: http://stackoverflow.com/questions/10766662/optimizing-nhibernate-session-factory-startup-time-of-webapp-really-slow
public class NHConfigurationFileCache
{
    private readonly string _cacheFile;
    private readonly Assembly _definitionsAssembly;

    public NHConfigurationFileCache(Assembly definitionsAssembly, HttpContext httpContext)
    {
        _definitionsAssembly = definitionsAssembly;
        _cacheFile = new ContextUtil(httpContext).GetFilePath("bin/nh.cfg");
    }

    public bool IsConfigurationFileValid
    {
        get
        {
            if (!File.Exists(_cacheFile))
                return false;

            var configInfo = new FileInfo(_cacheFile);
            var asmInfo = new FileInfo(_definitionsAssembly.Location);

            if (configInfo.Length < 5 * 1024)
                return false;

            return configInfo.LastWriteTime >= asmInfo.LastWriteTime;
        }
    }

    public void SaveConfigurationToFile(Configuration configuration)
    {
        var json = JsonSerializer.Serialize(configuration);
        File.WriteAllText(_cacheFile, json);
    }

    public Configuration LoadConfigurationFromFile()
    {
        if (!IsConfigurationFileValid)
            return null;

        var json = File.ReadAllText(_cacheFile);

        return JsonSerializer.Deserialize<Configuration>(json);
    }
}
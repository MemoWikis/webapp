using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using NHibernate.Cfg;

//see: http://stackoverflow.com/questions/10766662/optimizing-nhibernate-session-factory-startup-time-of-webapp-really-slow
public class NHConfigurationFileCache
{
    private readonly string _cacheFile;
    private readonly Assembly _definitionsAssembly;

    public NHConfigurationFileCache(Assembly definitionsAssembly)
    {
        _definitionsAssembly = definitionsAssembly;
        _cacheFile = "nh.cfg";
        if (HttpContext.Current != null) //for the web apps
            _cacheFile = HttpContext.Current.Server.MapPath(string.Format("~/bin/{0}", _cacheFile));
    }

    public void DeleteCacheFile()
    {
        if (File.Exists(_cacheFile))
            File.Delete(_cacheFile);
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
        using (var file = File.Open(_cacheFile, FileMode.Create))
        {
            var bf = new BinaryFormatter();
            bf.Serialize(file, configuration);
        }
    }

    public Configuration LoadConfigurationFromFile()
    {
        if (!IsConfigurationFileValid)
            return null;

        using (var file = File.Open(_cacheFile, FileMode.Open, FileAccess.Read))
        {
            var bf = new BinaryFormatter();
            return bf.Deserialize(file) as Configuration;
        }
    }
}
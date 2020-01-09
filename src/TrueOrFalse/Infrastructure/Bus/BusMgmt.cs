using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;

public class BusMgmt
{
    private static ManagementClient __client;
    static ManagementClient _client
    {
        get
        {
            if (__client != null)
                return __client;
            
            return __client = new ManagementClient("http://localhost", "guest", "guest");
        }
    }

    public static IEnumerable<Queue> GetQueues(){ return _client.GetQueues();}
    public static IEnumerable<Vhost> GetVHosts() { return _client.GetVHosts(); }

    public static void CreateTestQueue()
    {
        _client.CreateQueue(new QueueInfo("TestQueue"), GetVhost());        
    }

    private static Vhost GetVhost()
    {
        return _client.GetVhost("/");
    }
}
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Osc;

namespace Tool.Muse
{
    public class UdpReceiver
    {
        public static bool _shouldStop;

        public static void Start()
        {
            _shouldStop = false;
            var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Task.Run(() =>
            {
                using (var udpClient = new UdpClient(5000))
                {
                    int counter = 0;
                    while (!_shouldStop)
                    {
                        var receivedResults = udpClient.Receive(ref remoteEndPoint);
                        var content = Encoding.ASCII.GetString(receivedResults);

                        if (counter%500 == 0)
                            Log.Info(GetMessageString(receivedResults).ToString());
                            
                        if (content.StartsWith("/muse/elements/experimental/concentration"))
                            Log.Info(GetMessageString(receivedResults).ToString());

                        counter++;
                    }
                }
            });
        }

        private static Message GetMessageString(byte[] receivedResults)
        {
            var parser = new Parser();
            parser.FeedData(receivedResults);
            var message = parser.PopMessage();
            return message;
        }

        public static void Stop()
        {
            _shouldStop = true;
        }
    }
}

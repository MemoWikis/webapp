using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Osc;

namespace Tool.Muse
{
    public class UdpReceiver
    {
        public bool _shouldStop;

        public event EventHandler<OscMessage> OnReceive = delegate { };

        public void Start()
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
                        var message = GetMessageString(receivedResults);

                        if (counter % 3000 == 0)
                            Log.Info(message.ToString());

                        OnReceive(this, new OscMessage
                        {
                            Path = message.path,
                            DataRaw = message.data
                        });
                        
                        counter++;
                    }
                }
            });
        }

        private Message GetMessageString(byte[] receivedResults)
        {
            var parser = new Parser();
            parser.FeedData(receivedResults);
            var message = parser.PopMessage();
            return message;
        }

        public void Stop()
        {
            _shouldStop = true;
        }
    }

    
}

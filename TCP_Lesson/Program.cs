using System.Net;
using System.Net.Sockets;

namespace TCP_Lesson
{
    internal class Program
    {

        static TcpListener listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Server...");
            var ip = IPAddress.Parse("192.168.0.131");
            var port = 27001;
            var ep=new IPEndPoint(ip, port);

            listener = new TcpListener(ep);
            listener.Start();

            Console.WriteLine($"Listening: {listener.LocalEndpoint}");

            while (true)
            {
                var client = listener.AcceptTcpClient();
                Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

                //Client-den melumatlari qebul edib, cap edir

                Task.Run(() =>
                {
                    var reader = client.GetStream();
                    br=new BinaryReader(reader);

                    while (true)
                    {
                        var message = br.ReadString();
                        Console.WriteLine($"From client: {message}");
                    }
                });

                //Client-a mesaj gonderir

                var writer = Task.Run(() =>
                {
                    var stream = client.GetStream();
                    bw=new BinaryWriter(stream);

                    while (true)
                    {
                        var message = Console.ReadLine();
                        bw.Write(message ?? "NoMessage"); //eger mesaj yoxdusa noMessage yazsin
                    }
                });

            }


        }
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SoftUniHttpServer
{
    public class StartUp
    {
        private const int Port = 80;
        private const int Buffer = 10000;

        public static void Main(string[] args)
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, Port);
            tcpListener.Start();
            //

            while (true)
            {
                var client = tcpListener.AcceptTcpClient();

                using (NetworkStream stream = client.GetStream())
                {
                    var requestBytes = new byte[Buffer];
                    var readBytes = stream.Read(requestBytes, 0, requestBytes.Length);
                    var stringRequest = Encoding.UTF8.GetString(requestBytes, 0, readBytes);
                    Console.WriteLine(new string('=', 70));
                    Console.WriteLine(stringRequest);
                    var responseBody = "<form method='post'>input type='text' name='tweet' placeholder='Enter  tweet ...' /><input name='name' /><input type='submit' /></form>";
                    var response = "HTTP/1.0 200 OK" + Environment.NewLine
                        + "Content-Type: text/html" + Environment.NewLine +
                        "Server: MyCustomServer/1.0" + Environment.NewLine +
                        $"Content-Length: {responseBody.Length}" + Environment.NewLine + Environment.NewLine +
                        responseBody;
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);

                }

            }

        }
    }
}

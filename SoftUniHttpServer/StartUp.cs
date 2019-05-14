using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SoftUniHttpServer
{
    public class StartUp
    {
        private const int Port = 80;
        private const int Buffer = 10000;
        private const string NewLine = "\r\n";

        public static void Main(string[] args)
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, Port);
            tcpListener.Start();
            //
            StringBuilder stringBuilder = new StringBuilder();

            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                //Todo Mozila
                using (NetworkStream stream = client.GetStream())
                {
                    var requestBytes = new byte[Buffer];
                    Thread.Sleep(10000);
                    var readBytes = stream.Read(requestBytes, 0, requestBytes.Length);
                    var stringRequest = Encoding.ASCII.GetString(requestBytes, 0, readBytes);
                    Console.WriteLine(new string('=', 70));
                    Console.WriteLine(stringRequest);
                    var responseBody = "<form method='post'><input type='text' name='tweet' placeholder='Enter  tweet ...' /><input name='name' /><input type='submit' /></form>";
                    var response = "HTTP/1.0 307 Moved Temporary" + NewLine
                        + "Content-Type: text/html" + NewLine
                        + "Location: https://google.com"+ NewLine

                        + "Server: MyCustomServer/1.0" + NewLine +
                        $"Content-Length: {responseBody.Length}" + NewLine + NewLine +
                        responseBody;
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);

                }

            }

        }
    }
}

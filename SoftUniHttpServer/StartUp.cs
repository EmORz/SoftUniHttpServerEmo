using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoftUniHttpServer
{
    public class StartUp
    {
        private const int Port = 80;
        private const int Buffer = 10000;

        public static void Main(string[] args)
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 80);
            tcpListener.Start();
            //

            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                //Todo Mozila


                Task.Run(() =>
                ProcessingClient(client));
            }
        }

        public static async Task ProcessingClient(TcpClient client)
        {
             string NewLine = "\r\n";

            using (NetworkStream stream = client.GetStream())
            {
                var requestBytes = new byte[10000];
                // Thread.Sleep(10000);
                var readBytes = await stream.ReadAsync(requestBytes, 0, requestBytes.Length);
                var stringRequest = Encoding.UTF8.GetString(requestBytes, 0, readBytes);
                Console.WriteLine(new string('=', 70));
                Console.WriteLine(stringRequest);
                // var responseBody = "<form method='post'><input type='text' name='tweet' placeholder='Enter  tweet ...' /><input name='name' /><input type='submit' /></form>";
                var responseBody = DateTime.Now.ToString();
                var response = "HTTP/1.0 200 OK" + NewLine
                    + "Content-Type: text/html" + NewLine
                    + "Set-Cookie: cookie1=test" + NewLine
                    //+ "Location: https://google.com" + NewLine

                    + "Server: MyCustomServer/1.0" + NewLine +
                    $"Content-Length: {responseBody.Length}" + NewLine + NewLine +
                    responseBody;
                var responseBytes = Encoding.UTF8.GetBytes(response);
               await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

            }
        }
    }
}

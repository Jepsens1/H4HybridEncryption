using System.Net.Sockets;
using System.Net;
using System.Text;

namespace H4HybridEncryptionServer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Starting echo server...");

            int port = 1234;
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();

            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            RsaEncryption rsa = new RsaEncryption();
            AesEncryption aes = new AesEncryption();
            Console.WriteLine(rsa.SetClientKey(reader.ReadLine()));
            writer.WriteLine(Convert.ToBase64String(rsa.GenerateSessionKey()));

            while (true)
            {
                string inputLine = "";
                while (inputLine != null)
                {
                    inputLine = reader.ReadLine();
                    Console.WriteLine("Message from client: " + inputLine);
                    string test = aes.DecryptMessage(Convert.FromBase64String(inputLine),rsa.SessionKey);
                    Console.WriteLine("Decrypted string: " + test);
                }
                Console.WriteLine("Server saw disconnect from client.");
            }
        }
    }
}
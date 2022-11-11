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
            //We get the public key from client and saves it
            Console.WriteLine(rsa.SetClientKey(reader.ReadLine()));
            //Sends a rsa encrypted message with our SessionKey
            writer.WriteLine(Convert.ToBase64String(rsa.EncryptData(aes.SessionKey)));

            while (true)
            {
                string inputLine = "";
                while (inputLine != null)
                {
                    inputLine = reader.ReadLine();
                    Console.WriteLine("Message from client: " + inputLine);
                    //Decrypts the message we got from client and Display it
                    string decrypted = aes.DecryptMessage(Convert.FromBase64String(inputLine));
                    Console.WriteLine("Decrypted string: " + decrypted);
                    Console.Write("Enter text to send: ");
                    //Encrypt message and sends to client
                    byte[] messagebacktoclient = aes.EncryptMessage(Console.ReadLine());
                    Console.WriteLine("Sending to Client: " + Convert.ToBase64String(messagebacktoclient));
                    writer.WriteLine(Convert.ToBase64String(messagebacktoclient));
                }
                Console.WriteLine("Server saw disconnect from client.");
            }
        }
    }
}
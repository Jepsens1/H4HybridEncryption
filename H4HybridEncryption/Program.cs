

using System.Net.Sockets;
using System.Text;

namespace H4HybridEncryption
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Creating a TCPClient
            Console.WriteLine("Starting echo client...");

            int port = 1234;
            TcpClient client = new TcpClient("localhost", port);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
            //When Connected to Server we create a new object of RsaCrypto and we will send the server our publickey
            RsaCrypto rsa = new RsaCrypto();
            //Send public key to server
            writer.WriteLine(rsa.GetPublicKey());
            //In return the server responds back with a Rsa encrypted message that contains a AES key
            byte[] SessionKey = rsa.DecryptData(Convert.FromBase64String(reader.ReadLine()));

            while (true)
            {
                //Creates AES object
                AesCrypto aes = new AesCrypto();
                Console.Write("Enter text to send: ");
                //Calls AES encryption method to encrypt you text and saves in byte array
                byte[] message = aes.EncryptMessage(Console.ReadLine(), SessionKey);
                Console.WriteLine("Sending to server: " + Convert.ToBase64String(message));
                //Sends the encrypted byte array to server
                writer.WriteLine(Convert.ToBase64String(message));
                //We get a encrypted text back from server
                string lineReceived = reader.ReadLine();
                Console.WriteLine("Received from server: " + lineReceived);
                //Decrypts the message and display it to Console
                string decrypted = aes.DecryptMessage(Convert.FromBase64String(lineReceived), SessionKey);
                Console.WriteLine($"Decrypted: {decrypted}");
            }
        }
    }
}
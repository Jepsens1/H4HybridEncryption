

using System.Net.Sockets;
using System.Text;

namespace H4HybridEncryption
{
    public class Program
    {
        static void Main(string[] args)
        {

            //Console.WriteLine("Starting echo client...");

            //int port = 1234;
            //TcpClient client = new TcpClient("localhost", port);
            //NetworkStream stream = client.GetStream();
            //StreamReader reader = new StreamReader(stream);
            //StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            //while (true)
            //{
            //    Console.Write("Enter text to send: ");
            //    string lineToSend = Console.ReadLine();
            //    Console.WriteLine("Sending to server: " + lineToSend);
            //    writer.WriteLine(lineToSend);
            //    string lineReceived = reader.ReadLine();
            //    Console.WriteLine("Received from server: " + lineReceived);
            //}
            ICrypto rsa = new RsaCrypto();
            ICrypto aes = new AesCrypto();
            
            byte[] datatoEncrypt = Encoding.UTF8.GetBytes(Console.ReadLine());

            byte[] encryptedata = aes.EncryptData(datatoEncrypt);

            byte[] encryptedrsa = rsa.EncryptData(encryptedata);
            Console.WriteLine($"Encrypted data: {Encoding.UTF8.GetString(encryptedrsa)}");

            byte[] decryptedrsa = rsa.DecryptData(encryptedrsa);
            byte[] decryptedtext = aes.DecryptData(decryptedrsa);
            Console.WriteLine($"Decrypted data: {Encoding.UTF8.GetString(decryptedtext)}");

            Console.ReadLine();
        }
    }
}
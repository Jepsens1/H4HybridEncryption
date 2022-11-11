

using System.Net.Sockets;
using System.Text;

namespace H4HybridEncryption
{
    public class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Starting echo client...");

            int port = 1234;
            TcpClient client = new TcpClient("localhost", port);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
            RsaCrypto rsa = new RsaCrypto();
            writer.WriteLine(rsa.GetPublicKey());
            byte[] SessionKey = rsa.DecryptData(Convert.FromBase64String(reader.ReadLine()));

            while (true)
            {
                AesCrypto aes = new AesCrypto();
                Console.Write("Enter text to send: ");
                byte[] message = aes.EncryptMessage(Console.ReadLine(), SessionKey);
                Console.WriteLine("Sending to server: " + Convert.ToBase64String(message));
                writer.WriteLine(Convert.ToBase64String(message));
                string lineReceived = reader.ReadLine();
                Console.WriteLine("Received from server: " + lineReceived);
            }
            //    RsaCrypto rsa = new RsaCrypto();
            //    AesCrypto aes = new AesCrypto();

            //    byte[] datatoEncrypt = Encoding.UTF8.GetBytes(Console.ReadLine());

            //    byte[] encryptedata = aes.EncryptData(datatoEncrypt);

            //    byte[] encryptedrsa = rsa.EncryptData(encryptedata);
            //    Console.WriteLine($"Encrypted data: {Encoding.UTF8.GetString(encryptedrsa)}");

            //    byte[] decryptedrsa = rsa.DecryptData(encryptedrsa);
            //    byte[] decryptedtext = aes.DecryptData(decryptedrsa);
            //    Console.WriteLine($"Decrypted data: {Encoding.UTF8.GetString(decryptedtext)}");

            //    Console.ReadLine();
            //}
        }
    }
}
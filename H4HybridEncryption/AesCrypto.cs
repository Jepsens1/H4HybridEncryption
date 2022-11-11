using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace H4HybridEncryption
{
    public class AesCrypto
    {
        public string DecryptMessage(byte[] mess, byte[] sessionkey)
        {
            string plaintext = null;

            // Create an Aes object
            using (Aes aes = Aes.Create())
            {
                //Sets the AES key to the SessionKey we got from server
                aes.Key = sessionkey;
                using (MemoryStream msDecrypt = new MemoryStream(mess))
                {
                    //Reads the first 16 bytes from the message which is where the IV i stored
                    byte[] iv = new byte[aes.BlockSize / 8];
                    msDecrypt.Read(iv, 0, iv.Length);
                    //Sets the IV
                    aes.IV = iv;

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    // Create the streams used for decryption.
                    //using (MemoryStream msDecrypt = new MemoryStream(mess))
                    //{
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
        public byte[] EncryptMessage(string mess, byte[] sessionkey)
        {
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                //Sets key to be equal to SessionKey
                aes.Key = sessionkey;
                //Generates a IV with 16 random bytes
                aes.IV = GenerateRandomByteArray(16);

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Writes the IV, so when we decrypt we can get the first 16 bytes where IV i located
                            msEncrypt.Write(aes.IV);
                            //Write all data to the stream.
                            swEncrypt.Write(mess);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        public byte[] GenerateRandomByteArray(int size)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[size];
                rng.GetBytes(buffer);
                return buffer;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace H4HybridEncryption
{
    public class AesCrypto : ICrypto
    {
        Aes _symmetric;

        public AesCrypto()
        {
            _symmetric = Aes.Create();
            _symmetric.Padding = PaddingMode.Zeros;
            _symmetric.Key = GenerateRandomByteArray(32);
            _symmetric.IV = GenerateRandomByteArray(16);
        }
        public byte[] GetKey()
        {
            return _symmetric.Key;
        }
        public byte[] GetIv()
        {
            return _symmetric.IV;
        }
        public byte[] DecryptData(byte[] dataToDecrypt)
        {
            byte[] plaintext = new byte[dataToDecrypt.Length];
            MemoryStream ms = new MemoryStream(dataToDecrypt);
            CryptoStream cs = new CryptoStream(ms, _symmetric.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(plaintext, 0, dataToDecrypt.Length);
            cs.Close();
            return plaintext;
        }

        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, _symmetric.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            cs.Write(dataToEncrypt, 0, dataToEncrypt.Length);
            cs.Close();
            return ms.ToArray();
        }
        private byte[] GenerateRandomByteArray(int size)
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

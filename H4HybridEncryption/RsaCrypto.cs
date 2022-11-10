using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace H4HybridEncryption
{
    public class RsaCrypto : ICrypto
    {
        private RSACryptoServiceProvider rsa;
        private RSAParameters _privatekey;
        private RSAParameters _publickey;

        public RsaCrypto()
        {
            rsa = new RSACryptoServiceProvider(2048);
            _privatekey = rsa.ExportParameters(true);
            _publickey = rsa.ExportParameters(false);
        }
        public byte[] DecryptData(byte[] dataToDecrypt)
        {
            try
            {
                rsa.ImportParameters(_privatekey);
                return rsa.Decrypt(dataToDecrypt, true);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public string GetPublicKey()
        {
           return rsa.ToXmlString(false);
        }
        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            try
            {
                rsa.ImportParameters(_publickey);
                return rsa.Encrypt(dataToEncrypt, true);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}

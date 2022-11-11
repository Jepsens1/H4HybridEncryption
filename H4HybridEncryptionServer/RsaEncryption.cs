using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace H4HybridEncryptionServer
{
    public class RsaEncryption
    {
        private RSACryptoServiceProvider rsa;
        private RSAParameters _privatekey;
        public RSAParameters _publickey;
        public RSAParameters _clientkey;
        public byte[] SessionKey;

        public RsaEncryption()
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
        public string SetClientKey(string key)
        {
            StringReader reader = new StringReader(key);
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            _clientkey = (RSAParameters)xs.Deserialize(reader);

            return "Got public key from client";
        }

        public byte[] GenerateSessionKey()
        {

            using (Aes aes = Aes.Create())
            {
                SessionKey = aes.Key;
            }

            return EncryptData(SessionKey);
        }
        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            try
            {
                rsa.ImportParameters(_clientkey);
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

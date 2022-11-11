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
                //Import our private key when trying to decrypt
                rsa.ImportParameters(_privatekey);
                return rsa.Decrypt(dataToDecrypt, false);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public string SetClientKey(string key)
        {
            //We take the xml string we got from client and Deserialize and sets _clientkey to that
            StringReader reader = new StringReader(key);
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            _clientkey = (RSAParameters)xs.Deserialize(reader);

            return "Got public key from client";
        }
        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            try
            {
                //Import our ClientKey
                rsa.ImportParameters(_clientkey);
                return rsa.Encrypt(dataToEncrypt, false);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}

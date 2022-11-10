using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H4HybridEncryption
{
    public interface ICrypto
    {
        public byte[] EncryptData(byte[] dataToEncrypt);

        public byte[] DecryptData(byte[] dataToDecrypt);
    }
}

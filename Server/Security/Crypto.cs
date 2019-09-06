using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Security
{
    public class Crypto
    {
        private const string initVector = "deb3riagpR1pnh54";
        private const string passPhrase = "a3x2i7das5a";
        private const int keysize = 256;

        public static string Encrypt(string stringToEncrypt)
        {
            try
            {
                if (stringToEncrypt == null || stringToEncrypt.Trim() == "")
                    return stringToEncrypt;

                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(stringToEncrypt);
                var password = new PasswordDeriveBytes(passPhrase, null);
                password.IterationCount = 126;
                byte[] keyBytes = password.GetBytes(keysize / 8);
                var symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                var memoryStream = new MemoryStream();
                var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch
            {
                return stringToEncrypt;
            }
        }

        public static string Decrypt(string stringToDecrypt)
        {
            try
            {
                if (stringToDecrypt == null || stringToDecrypt.Trim() == "")
                    return stringToDecrypt;

                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(stringToDecrypt);
                var password = new PasswordDeriveBytes(passPhrase, null);
                password.IterationCount = 126;
                byte[] keyBytes = password.GetBytes(keysize / 8);
                var symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch
            {
                return stringToDecrypt;
            }
        }

    }
}

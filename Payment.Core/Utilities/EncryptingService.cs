
using Microsoft.Extensions.Configuration;
using Payment.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Payment.Core.Utilities
{
    public class EncryptingService : IEncryptingService
    {
        private readonly IConfiguration _config;
        private const int Keysize = 256;
        private const int BlockSize = 128;
        private static readonly Encoding encoding = Encoding.UTF8;

        public EncryptingService(IConfiguration config)
        {
            _config = config;
        }

        public string AES_decrypt(string encrypted)
        {
            var keys = _config.GetSection("ConfiguredApps");
            string Key = keys["EncKey"]; string Iv = keys["EncIV"];
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encrypted);
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;// Not Required
                aes.KeySize = 256; //Not Required
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(Iv);
                ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] secret = crypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                crypto.Dispose();
                return Encoding.ASCII.GetString(secret);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string AES_Encrypt(string plainText)
        {
            var keys = _config.GetSection("ConfiguredApps");
            string Key = keys["EncKey"]; string Iv = keys["EncIV"];
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(plainText));
            }

            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(Iv);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }
    }
}

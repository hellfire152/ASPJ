using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ASPJ_Project.Models
{
    public class Crypto
    {
        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }

    public class AESCryptoStuff
    {
        public static AESCryptoStuff CurrentInstance;

        AesCryptoServiceProvider cryptProvider;

        public static void Initialize(string IV, string key)
        {
            AesCryptoServiceProvider p = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 256,
                IV = Encoding.UTF8.GetBytes(IV),
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            CurrentInstance = new AESCryptoStuff(p);
        }

        public AESCryptoStuff(AesCryptoServiceProvider p)
        {
            this.cryptProvider = p;
        }

        public String AesEncrypt(String clear_text)
        {
            ICryptoTransform transform = cryptProvider.CreateEncryptor();
            byte[] encrypted_bytes = transform.TransformFinalBlock(ASCIIEncoding.ASCII.GetBytes(clear_text), 0, clear_text.Length);
            string str = Convert.ToBase64String(encrypted_bytes);
            return str;
        }

        public String AesDecrypt(String cipher_text)
        {
            ICryptoTransform transform = cryptProvider.CreateDecryptor();
            byte[] enc_bytes = Convert.FromBase64String(cipher_text);
            byte[] decrypted_bytes = transform.TransformFinalBlock(enc_bytes, 0, enc_bytes.Length);
            string str = ASCIIEncoding.ASCII.GetString(decrypted_bytes);
            return str;
        }
    }


}
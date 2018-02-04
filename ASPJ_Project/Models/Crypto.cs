using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class Crypto
    {
        public static Crypto CurrentInstance;
        public Crypto(string cookieKey)
        {

        }
        public string Encrypt(string message)
        {
            return message;
        }

        public string Decrypt(string message)
        {
            return message;
        }

        public string EncryptCookie(string message)
        {
            return message;
        }

        public string DecryptCookie(string message)
        {
            return message;
        }

        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ASPJ_Project.Models
{
    public class Hmac
    {
        public static Hmac CurrentInstance;

        public static void Initialize(string key)
        {
            byte[] keyArr = Encoding.ASCII.GetBytes(key);
            CurrentInstance = new Hmac(keyArr);
        }

        private HMACSHA1 h;

        public Hmac(byte[] key)
        {
            h = new HMACSHA1(key);
        }

        //return a base 32 encoded string of the input
        public string Encode(string input)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(input);
            byte[] encodedBytes = h.ComputeHash(messageBytes);
            return Base32.Encode(encodedBytes);
        }
    }
}
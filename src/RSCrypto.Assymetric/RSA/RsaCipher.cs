using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RSCrypto.Assymetric.RSA
{
    public static class RsaCipher
    {

        public static byte[] Encrypt(byte[] data, RsaKeyBase key, RSAEncryptionPadding padding = null)
        {
            if (padding == null)
                padding = RSAEncryptionPadding.OaepSHA1;

            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.KeySize = key.Length;
                rsa.ImportParameters(key.Parameters);
                return rsa.Encrypt(data, padding);
            }
        }

        public static string Encrypt(string data, RsaKeyBase key, RSAEncryptionPadding padding = null)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var cdata = Encrypt(bytes, key, padding);
            return Convert.ToBase64String(cdata);
        }

        public static byte[] Decrypt(byte[] data, RsaKeyBase key, RSAEncryptionPadding padding = null)
        {
            if (padding == null)
                padding = RSAEncryptionPadding.OaepSHA1;

            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.KeySize = key.Length;
                rsa.ImportParameters(key.Parameters);
                return rsa.Decrypt(data, padding);
            }
        }

        public static string Decrypt(string data, RsaKeyBase key, RSAEncryptionPadding padding = null)
        {
            var bytes = Convert.FromBase64String(data);
            var cdata = Decrypt(bytes, key, padding);
            return Encoding.UTF8.GetString(cdata);
        }
    }
}

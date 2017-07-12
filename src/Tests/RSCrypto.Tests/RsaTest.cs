using System;
using System.IO;
using RSCrypto.Assymetric.RSA;
using Xunit;

namespace RSCrypto.Tests
{
    public class RsaTest : IDisposable
    {
        private const string PairFile = "pair.xml";
        [Fact]
        public void CreateKeyPair()
        {
            var pair = new RsaKeyPair();

            Assert.NotNull(pair.PublicKey);
            Assert.NotNull(pair.PrivateKey);
        }

        [Fact]
        public void SaveAndLoad()
        {
            var pair = new RsaKeyPair();
            pair.SaveToFile(PairFile);

            var pair2 = new RsaKeyPair();
            pair2.LoadFromFile(PairFile);

            Assert.Equal(pair.PublicKey.Parameters.Exponent, pair2.PublicKey.Parameters.Exponent);
            Assert.Equal(pair.PublicKey.Parameters.Modulus, pair2.PublicKey.Parameters.Modulus);
            Assert.Equal(pair2.PublicKey.Parameters.D, null);
        }

        [Fact]
        public void EncryptAndDecrypt()
        {
            var pair = new RsaKeyPair();

            var str = "Hello";

            var encoded = RsaCipher.Encrypt(str, pair.PublicKey);
            Assert.NotEqual(str, encoded);

            var decoded = RsaCipher.Decrypt(encoded, pair.PrivateKey);
            Assert.Equal(str, decoded);


        }

        public void Dispose()
        {
            if (File.Exists(PairFile))
                File.Delete(PairFile);
        }
    }
}

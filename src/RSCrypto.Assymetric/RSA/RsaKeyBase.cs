using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.IO;

namespace RSCrypto.Assymetric.RSA
{
    public abstract class RsaKeyBase
    {
        public RSAParameters Parameters { get; protected set; }
        public int Length { get; protected set; }

        protected RsaKeyBase(RSAParameters parameters) : base()
        {
            Parameters = parameters;
        }

        protected RsaKeyBase(string key) : base()
        {
            LoadFromString(key);
        }

        public override string ToString()
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(ToXmlString());
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public void LoadFromString(string encoded)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encoded);
            var xml = Encoding.UTF8.GetString(base64EncodedBytes);
            LoadFromXmlString(xml);
        }

        public void SaveToFile(string filename)
        {
            File.WriteAllText(filename, ToXmlString(), Encoding.UTF8);
        }

        public void LoadFromFile(string filename)
        {
            LoadFromXmlString(File.ReadAllText(filename, Encoding.UTF8));
        }


        public virtual string ToXmlString()
        {
            throw new NotImplementedException(@"This method must be overridden");
        }

        
        public virtual void LoadFromXmlString(string xmlString)
        {
            throw new NotImplementedException(@"This method must be overridden");
        }
    }
}

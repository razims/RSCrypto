using System;
using System.IO;
using System.Text;
using System.Xml;

namespace RSCrypto.Assymetric.RSA
{
    public class RsaKeyPair
    {
        public RsaPrivateKey PrivateKey { get; protected set; }
        public RsaPublicKey PublicKey { get; protected set; }

        public int KeyLength { get; protected set; }

        public RsaKeyPair() : this(1024)
        {
            
        }

        public RsaKeyPair(int keyLength) 
        {
            KeyLength = keyLength;
            using (var rsa = System.Security.Cryptography.RSA.Create())
            {
                rsa.KeySize = keyLength;
                PublicKey = new RsaPublicKey(rsa.ExportParameters(false), keyLength);
                PrivateKey = new RsaPrivateKey(rsa.ExportParameters(true), keyLength);
            }
        }

        public RsaKeyPair(string key) 
        {
            LoadFromString(key);
        }


        public override string ToString()
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(ToXmlString());
            return Convert.ToBase64String(plainTextBytes);
        }

        public void LoadFromString(string encoded)
        {
            var base64EncodedBytes = Convert.FromBase64String(encoded);
            var xml = Encoding.UTF8.GetString(base64EncodedBytes);
            LoadFromXmlString(xml);
        }

        public virtual string ToXmlString()
        {
            var keyLength = string.Format("<{0}>{1}</{0}>", RsaKeyTagType.KeyLength, KeyLength);
            return string.Format("<{0}>{1}{2}{3}</{0}>", RsaKeyTagType.KeyPair, PublicKey.ToXmlString(), PrivateKey.ToXmlString(), keyLength);
        }

        public virtual void LoadFromXmlString(string xmlString)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals(RsaKeyTagType.KeyPair))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (node.Name == RsaKeyTagType.Public)
                    {
                        PublicKey = new RsaPublicKey();
                        PublicKey.LoadFromXmlString(node.OuterXml);
                    }
                    else if (node.Name == RsaKeyTagType.Private)
                    {
                        PrivateKey = new RsaPrivateKey();
                        PrivateKey.LoadFromXmlString(node.OuterXml);
                    }
                    else if (node.Name == RsaKeyTagType.KeyLength)
                    {
                        KeyLength = Int32.Parse(node.InnerText);
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }
        }

        public void SaveToFile(string filename)
        {
            File.WriteAllText(filename, ToXmlString(), Encoding.UTF8);
        }

        public void LoadFromFile(string filename)
        {
            LoadFromXmlString(File.ReadAllText(filename, Encoding.UTF8));
        }

    }
}

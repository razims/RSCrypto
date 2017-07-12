using System;
using System.Security.Cryptography;
using System.Xml;
using RSCrypto.Assymetric.Extensions;

namespace RSCrypto.Assymetric.RSA
{
    public class RsaPublicKey : RsaKeyBase
    {
        public RsaPublicKey() : base(new RSAParameters())
        {
        }

        public RsaPublicKey(RSAParameters parameters, int length) : base(parameters)
        {
            Length = length;
        }

        public RsaPublicKey(string key) : base(key)
        {
        }

        public override string ToXmlString()
        {
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement(RsaKeyTagType.Public);

            xmlDoc.AppendChild(root);
            xmlDoc.AppendElementWithByteContent(root, "Modulus", Parameters.Modulus)
                .AppendElementWithByteContent(root, "Exponent", Parameters.Exponent)
                .AppendElementWithByteContent(root, RsaKeyTagType.KeyLength, Length);

            return xmlDoc.OuterXml;

        }

        public override void LoadFromXmlString(string xmlString)
        {
            var parameters = new RSAParameters();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals(RsaKeyTagType.Public))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "Length": Length = Int32.Parse(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            this.Parameters = parameters;
        }
    }
}
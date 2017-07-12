using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using RSCrypto.Assymetric.Extensions;

namespace RSCrypto.Assymetric.RSA
{
    public class RsaPrivateKey : RsaKeyBase
    {

        public RsaPrivateKey() : base(new RSAParameters())
        {
        }

        public RsaPrivateKey(RSAParameters parameters, int length) : base(parameters)
        {
            Length = length;
        }

        protected RsaPrivateKey(string key) : base(key)
        {
        }

        public override string ToXmlString()
        {

            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement(RsaKeyTagType.Private);

            xmlDoc.AppendChild(root);
            xmlDoc.AppendElementWithByteContent(root, "Modulus", Parameters.Modulus)
                .AppendElementWithByteContent(root, "Exponent", Parameters.Exponent)
                .AppendElementWithByteContent(root, "P", Parameters.P)
                .AppendElementWithByteContent(root, "Q", Parameters.Q)
                .AppendElementWithByteContent(root, "DP", Parameters.DP)
                .AppendElementWithByteContent(root, "DQ", Parameters.DQ)
                .AppendElementWithByteContent(root, "InverseQ", Parameters.InverseQ)
                .AppendElementWithByteContent(root, "D", Parameters.D)
                .AppendElementWithByteContent(root, RsaKeyTagType.KeyLength, Length);

            return xmlDoc.OuterXml;

            

            /*return string.Format("<RSAPrivateKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAPrivateKeyValue>",
                Convert.ToBase64String(Parameters.Modulus),
                Convert.ToBase64String(Parameters.Exponent),
                Convert.ToBase64String(Parameters.P),
                Convert.ToBase64String(Parameters.Q),
                Convert.ToBase64String(Parameters.DP),
                Convert.ToBase64String(Parameters.DQ),
                Convert.ToBase64String(Parameters.InverseQ),
                Convert.ToBase64String(Parameters.D));*/
        }

        public override void LoadFromXmlString(string xmlString)
        {
            var parameters = new RSAParameters();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals(RsaKeyTagType.Private.ToString()))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
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

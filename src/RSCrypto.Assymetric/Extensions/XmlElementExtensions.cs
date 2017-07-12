using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;

namespace RSCrypto.Assymetric.Extensions
{
    internal static class XmlDocumentExtensions
    {
        public static XmlDocument AppendElementWithByteContent(this XmlDocument xmlDoc, XmlElement root, string name, byte[] bytes)
        {
            var text = Convert.ToBase64String(bytes);

            return xmlDoc.AppendElementWithContent(root, name, text);
        }

        public static XmlDocument AppendElementWithByteContent(this XmlDocument xmlDoc, XmlElement root, string name, int num)
        {
            var text = num.ToString();

            return xmlDoc.AppendElementWithContent(root, name, text);
        }

        public static XmlDocument AppendElementWithContent(this XmlDocument xmlDoc, XmlElement root, string name, string  content)
        {
            var textNode = xmlDoc.CreateTextNode(content);
            var element = xmlDoc.CreateElement(name);
            element.AppendChild(textNode);
            root.AppendChild(element);

            return xmlDoc;  
        }
    }
}

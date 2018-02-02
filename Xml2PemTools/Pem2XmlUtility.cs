using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Xml2PemTools
{
    internal static class Pem2XmlUtility
    {
        public static void XmlConvertToPem(string xmlPath, string pemPath)//XML格式密钥转PEM
        {
            var rsa2 = new RSACryptoServiceProvider();
            using (var sr = new StreamReader(xmlPath))
            {
                rsa2.FromXmlString(sr.ReadToEnd());
            }
            var p = rsa2.ExportParameters(true);

            var key = new RsaPrivateCrtKeyParameters(
                new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent), new BigInteger(1, p.D),
                new BigInteger(1, p.P), new BigInteger(1, p.Q), new BigInteger(1, p.DP), new BigInteger(1, p.DQ),
                new BigInteger(1, p.InverseQ));

            using (var sw = new StreamWriter(pemPath))
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
                pemWriter.WriteObject(key);
            }
        }

        public static void PemConvertToXml(string pemPath, string xmlPath)//PEM格式密钥转XML
        {
            AsymmetricCipherKeyPair keyPair;
            using (var sr = new StreamReader(pemPath))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            }
            var key = (RsaPrivateCrtKeyParameters)keyPair.Private;
            var p = new RSAParameters
            {
                Modulus = key.Modulus.ToByteArrayUnsigned(),
                Exponent = key.PublicExponent.ToByteArrayUnsigned(),
                D = key.Exponent.ToByteArrayUnsigned(),
                P = key.P.ToByteArrayUnsigned(),
                Q = key.Q.ToByteArrayUnsigned(),
                DP = key.DP.ToByteArrayUnsigned(),
                DQ = key.DQ.ToByteArrayUnsigned(),
                InverseQ = key.QInv.ToByteArrayUnsigned(),
            };
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);
            using (var sw = new StreamWriter(xmlPath))
            {
                sw.Write(rsa.ToXmlString(true));
            }
        }
    }
}

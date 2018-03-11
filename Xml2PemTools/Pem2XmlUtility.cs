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
        public static void XmlConvertToPem(string xmlPath, string pemPath, bool generatePrivateKey = true)//XML格式密钥转PEM
        {
            var rsa2 = new RSACryptoServiceProvider();
            rsa2.FromXmlString(File.ReadAllText(xmlPath));
            var p = rsa2.ExportParameters(!rsa2.PublicOnly);

            //Public Key Convert to Private Key
            if (rsa2.PublicOnly)
            {
                generatePrivateKey = false;
            }

            AsymmetricKeyParameter key = null;
            if (generatePrivateKey)
            {
                //Private Key
                key = new RsaPrivateCrtKeyParameters(
                    new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent), new BigInteger(1, p.D),
                    new BigInteger(1, p.P), new BigInteger(1, p.Q), new BigInteger(1, p.DP), new BigInteger(1, p.DQ),
                    new BigInteger(1, p.InverseQ));
            }
            else
            {
                //Public key
                key = new RsaKeyParameters(false, new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent)); //Public Key
            }

            using (var sw = new StreamWriter(pemPath))
            {
                var pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(sw);
                pemWriter.WriteObject(key);
            }
        }

        public static void PemConvertToXml(string pemPath, string xmlPath, bool generatePrivateKey = true)//PEM格式密钥转XML
        {
            RSAParameters p;
            using (var sr = File.OpenText(pemPath))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(sr);
                var obj = pemReader.ReadObject();
                if (obj is RsaKeyParameters)
                {
                    var key = obj as RsaKeyParameters;
                    p = new RSAParameters
                    {
                        Modulus = key.Modulus.ToByteArrayUnsigned(),
                        Exponent = key.Exponent.ToByteArrayUnsigned()
                    };

                    //Public Key cant Convert To Private Key
                    generatePrivateKey = false;
                }
                else if (obj is AsymmetricCipherKeyPair)
                {
                    var key = (obj as AsymmetricCipherKeyPair).Private as RsaPrivateCrtKeyParameters;
                    p = new RSAParameters
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
                }
                else
                {
                    throw new NotSupportedException("not support this pem");
                }
            }
            
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(p);
            File.WriteAllText(xmlPath, rsa.ToXmlString(generatePrivateKey)); //True: PrivateKey, False PublicKey
        }
    }
}

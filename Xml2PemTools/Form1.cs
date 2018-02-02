using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Xml2PemTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Pem2XmlUtility.PemConvertToXml(@"D:\PrivateKey.pem", @"D:\PrivateKey.xml");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Pem2XmlUtility.XmlConvertToPem(@"D:\PrivateKey.xml", @"D:\PrivateKey2.pem");
        }

        // ref https://www.jianshu.com/p/faefcc58c79b
    }
}

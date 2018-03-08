using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            var openDlgResult = openFileDialog1.ShowDialog();
            switch (openDlgResult)
            {
                case DialogResult.OK:
                    if (string.IsNullOrEmpty(openFileDialog1.FileName))
                    {
                        MessageBox.Show("文件不存在", "PemXmlConverter");
                        return;
                    }

                    break;
                case DialogResult.Cancel:
                    return;
            }
            

            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(openFileDialog1.FileName) + ".xml";
            var saveDlgResult = saveFileDialog1.ShowDialog();
            switch (saveDlgResult)
            {
                case DialogResult.OK:
                    if (string.IsNullOrWhiteSpace(saveFileDialog1.FileName))
                    {
                        MessageBox.Show("请选择正确的位置保存文件", "PemXmlConverter");
                        return;
                    }

                    break;
                case DialogResult.Cancel:
                    return;
            }
            

            Pem2XmlUtility.PemConvertToXml(openFileDialog1.FileName, saveFileDialog1.FileName);
            Process.Start("explorer.exe", "/SELECT," + saveFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var openDlgResult = openFileDialog2.ShowDialog();
            switch (openDlgResult)
            {
                case DialogResult.OK:
                    if (string.IsNullOrEmpty(openFileDialog2.FileName))
                    {
                        MessageBox.Show("文件不存在", "PemXmlConverter");
                        return;
                    }

                    break;
                case DialogResult.Cancel:
                    return;
            }


            saveFileDialog2.FileName = Path.GetFileNameWithoutExtension(openFileDialog2.FileName) + ".pem";
            var saveDlgResult = saveFileDialog2.ShowDialog();
            switch (saveDlgResult)
            {
                case DialogResult.OK:
                    if (string.IsNullOrWhiteSpace(saveFileDialog2.FileName))
                    {
                        MessageBox.Show("请选择正确的位置保存文件", "PemXmlConverter");
                        return;
                    }

                    break;
                case DialogResult.Cancel:
                    return;
            }

            Pem2XmlUtility.XmlConvertToPem(openFileDialog2.FileName, saveFileDialog2.FileName);
            Process.Start("explorer.exe", "/SELECT," + saveFileDialog2.FileName);
        }

        // ref https://www.jianshu.com/p/faefcc58c79b
        // in repo
    }
}

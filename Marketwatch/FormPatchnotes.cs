using System;

using System.Windows.Forms;
using System.Xml;
using UpdateLib;

namespace Marketwatch {
    public partial class FormPatchnotes : Form {
        private const string rtfHeader = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1031{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft Sans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17 ";
        private const string rtfTrailer = "\\par\r\n}\r\n";

        private string rtfDescription = rtfHeader + "\\fs32\\b Marketwatch Relase Notes \\b0\\fs17\\par\\par ";

        public FormPatchnotes(ISharpUpdatable context) {
            InitializeComponent();

            XmlDocument doc = new XmlDocument();
            doc.Load(context.UpdateXmlLocation.AbsoluteUri);

            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("//update[@appId='" + context.ApplicationID + "']");

            foreach (XmlNode node in nodeList) {
                rtfDescription += "\\fs22\\b Version " + node["version"].InnerText + " \\b0\\par ";
                rtfDescription += "\\fs17" + node["description"].InnerText + " \\par\\par ";
            }

            rtfDescription += rtfTrailer;

        }

        private void FormPatchnotes_Load(object sender, EventArgs e) {
            richTextBox1.Rtf = rtfDescription;
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e) {
            // Only allow Cntrl - C to copy text
            if (!(e.Control && e.KeyCode == Keys.C))
                e.SuppressKeyPress = true;
        }
    }
}

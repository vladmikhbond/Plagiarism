using Plagiarism;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hunter
{
    public partial class MainForm : Form
    {
        XComparer comparer = new XComparer();

        public MainForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                Report(openFileDialog1.FileName);

            }
        }

        private void Report(string manualName)
        {
            comparer.Manuscript = File.ReadAllText(manualName);

            string dirName = Path.GetDirectoryName(manualName);
            reportBox.Text = "";
            foreach (string bookName in Directory.GetFiles(dirName))
            {
                if (bookName == manualName)
                    continue;
                var report = comparer.Compare(Path.GetFileName(bookName), File.ReadAllText(bookName));
                foreach (var item in report)
                {
                    reportBox.Text += item.ToString() + "\r\n";
                }
                reportBox.Text += "\n";
            }
        }
    }
}

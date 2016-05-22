using Plagiarism;
using System;
using System.IO;
using System.Windows.Forms;

namespace Hunter
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openManuscriptDialog.ShowDialog() == DialogResult.OK)
            {
                int delta;
                try
                {
                    delta = Convert.ToInt32(toolStripDelta.Text);
                } catch
                {
                    delta = 100;
                    toolStripDelta.Text = "100 ";
                }
                textFragmentBox.Text = "WAIT...";
                textFragmentBox.Refresh();
                DoReport(openManuscriptDialog.FileName, delta);
                textFragmentBox.Text = "READY.";
            }
        }

        private void DoReport(string manualPath, int n)
        {
            string s = File.ReadAllText(manualPath);
            Book manual = new Book("", s);

            XComparer comparer = new XComparer(manual.Digest, n);

            string root = Path.GetDirectoryName(manualPath);


            reportBox.Items.Clear();
            foreach (string bookPath in Directory.GetFiles(root))
            {
                if (bookPath == manualPath)
                    continue;

                Book book = new Book(
                    name: Path.GetFileName(bookPath),
                    source: File.ReadAllText(bookPath));

                var report = comparer.Compare(book.Digest);

                foreach (var reportItem in report)
                {
                    reportItem.Book = book;
                    reportBox.Items.Add(reportItem);
                }
            }
            textFragmentBox.Text = "";
        }

        private void reportBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportBox.SelectedIndex != -1)
            {
                var item = (ReportItem)reportBox.SelectedItem;
                textFragmentBox.Text = item.Fragment;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

﻿using Hunter.Properties;
using Plagiarism;
using System;
using System.Diagnostics;
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
                Text = $"Hunter - {Path.GetFileName(openManuscriptDialog.FileName)}";
                int delta;
                try
                {
                    delta = Convert.ToInt32(toolStripDelta.Text);
                } catch
                {
                    delta = 100;
                    toolStripDelta.Text = "100 ";
                }
                Stopwatch sw = new Stopwatch();
                sw.Start();

                textFragmentBox.Text = "WAIT...";
                textFragmentBox.Refresh();
                DoReport(openManuscriptDialog.FileName, delta);
                sw.Stop();

                textFragmentBox.Text = $"READY. Elapsed time={sw.Elapsed}";
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
                    reportItem.Manual = manual;
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
                textFragmentBox.Text = item.GetBookFragment();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void reportBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (reportBox.SelectedIndex != -1)
            {
                var item = (ReportItem)reportBox.SelectedItem;
                textFragmentBox.Text = $"Manuscript:\r\n{item.GetBookFragment()}\r\n\r\nBook:\r\n{item.GetManualFragment()}";
            }

        }

        private void openManuscriptDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}

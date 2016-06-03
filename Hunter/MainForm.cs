using Hunter.Properties;
using Plagiarism;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Hunter
{
    public partial class MainForm : Form
    {
        string manualPath;

        public MainForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openManuscriptDialog.ShowDialog() == DialogResult.OK)
            {
                manualPath = openManuscriptDialog.FileName;
                Text = $"{Path.GetFileName(manualPath)} - Hunter";
                AnalizeWithWatch();
            }
        }


        private void toolStripDelta_Leave(object sender, EventArgs e)
        {
            AnalizeWithWatch();
        }

        private void AnalizeWithWatch()
        {
            if (string.IsNullOrWhiteSpace(manualPath))
                return;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            textFragmentBox.Text = "WAIT...";
            textFragmentBox.Refresh();

            var max = Analize();
            sw.Stop();

            textFragmentBox.Text = $"Elapsed time = {sw.Elapsed} \r\nMax = {max}";
        }


        private int Analize()
        {
            Book manual = new Book(name:"", source:File.ReadAllText(manualPath));

            XComparer comparer = new XComparer(manual.Digest, GetFrigmentSize());

            string rootPath = Path.GetDirectoryName(manualPath);

            int max = 0;
            reportBox.Items.Clear();
            foreach (string bookPath in Directory.GetFiles(rootPath, "*.txt"))
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

                if (report.Count > 0)
                    max = Math.Max(max, report.Max(r => r.Length));
            }
            return max;
        }


        private int GetFrigmentSize()
        {
            int result;
            if (int.TryParse(toolStripDelta.Text, out result))
            {
                return result;
            }
            toolStripDelta.Text = "100";
            return 100;
        }


        // Show selected text fragment
        //
        private void reportBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportBox.SelectedIndex != -1)
            {
                var item = (ReportItem)reportBox.SelectedItem;
                textFragmentBox.Text = item.GetBookFragment();
            }
        }

        // Show a pair of fragments
        //
        private void reportBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (reportBox.SelectedIndex != -1)
            {
                var item = (ReportItem)reportBox.SelectedItem;

                textFragmentBox.Text =
$@"{item.GetBookFragment()}

============================ {item.Book.Name}: =================================

{item.GetManualFragment()}";

            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        // Change the font size
        //
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta > 0)
                Font = new System.Drawing.Font(Font.FontFamily, Font.Size + 0.2f);
            else
                Font = new System.Drawing.Font(Font.FontFamily, Font.Size - 0.2f);
        }


        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textFragmentBox.Text =
@"● Проверке подвергаются текстовые файлы (*.txt) в кодировке UTF-8.

● Все файлы, включая проверяемый, должны находиться в одной папке.  

● Для проверки файла нужно открыть его в меню File / Open.

● Минимальная величина фрагмента заимствования устанавливается в поле Gap в области главного меню.

● Отчет о проверке выдается в виде списка заимствованных фрагментов в верхней части формы.

● Выбранный в списке фрагмент будет виден в текстовом поле в нижней части формы.

● Двойной клик по выбранному фрагменту показывает копию и оригинал фрагмента.

";
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.toolStripDelta.Text = Settings.Default.GapSize;
        }

        private void toolStripDelta_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.GapSize = this.toolStripDelta.Text;
            Settings.Default.Save();
        }

        private void repeatToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripDelta_Click(object sender, EventArgs e)
        {

        }
    }
}

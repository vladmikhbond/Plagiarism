using Hunter.Properties;
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

                textFragmentBox.Text = $"Elapsed time = {sw.Elapsed}";
            }
        }

        private void DoReport(string manualPath, int n)
        {
            string s = File.ReadAllText(manualPath);
            Book manual = new Book("", s);

            XComparer comparer = new XComparer(manual.Digest, n);

            string root = Path.GetDirectoryName(manualPath);


            reportBox.Items.Clear();
            foreach (string bookPath in Directory.GetFiles(root, "*.txt"))
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

                textFragmentBox.Text = 
$@"{item.GetBookFragment()}

============================ {item.Book.Name}: =================================

{item.GetManualFragment()}";

            }

        }

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

        private void toolStripDelta_Leave(object sender, EventArgs e)
        {
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
    }
}

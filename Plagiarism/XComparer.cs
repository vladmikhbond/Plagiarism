using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism
{
    /// <summary>
    /// Compare a manuscript with books from library with List<ReporItem> as result.
    /// N - step of testing, N/2 - initial sample size
    /// </summary>
    public class XComparer
    {
        public int N { set; get; }

        // It is analized.
        public string  Manuscript { set; private get; }

        public XComparer(string manuscript, int n)
        {
            Manuscript = manuscript;
            N = n;
        }

        public XComparer() : this("", 100) { }

        // Compare the manuscript with a book
        // 
        public List<ReportItem> Compare(string bookName, string bookText)
        {
            var result = new List<ReportItem>();
            int pos = 0;
            // отбираем пробы в манускрипте с шагом N и длиной N/2
            while (pos < Manuscript.Length - N)
            {
                // ищем расширенную пробу в книге 
                var item = IndexOfMax(bookText, pos);
                
                if (item != null)
                {
                    item.BookName = bookName;
                    result.Add(item);
                    pos += item.Length; 
                }
                pos += N;
            }
            return result;
        }

        // Ищет вхождения части манускрипта в книгу. Часть начинается с позиции pos и имеен длину N символов.
        // Находит только первое вхождение, хотя должен искать максимальное из всех.
        //
        public ReportItem IndexOfMax(string book, int pos) {
            string sample = Manuscript.Substring(pos, N);
            int ib = Find (book, sample, 0);
            // если проба нашлась, расширяем ее до максимального размера
            if (ib > -1)
            {
                int im = ExtStart(book, ib, pos);
                int jm = ExtFinish(book, ib + N, pos + N);
                return new ReportItem { Start = im, Length = jm - im };
            }
            return null;
        }

        // Сдвигаем начало 
        //
        public int ExtStart(string bookText, int ib, int im)
        {
            while (im >= 0 && ib >= 0 && bookText[ib] == Manuscript[im])
            {
                ib--;
                im--;
            };
            return im + 1;
        }

        // Сдвигаем конец 
        //
        public int ExtFinish(string bookText, int ib, int im)
        {
            while (ib < bookText.Length && im < Manuscript.Length && bookText[ib] == Manuscript[im])
            {
                ib++;
                im++;
            }
            return im;
        }


        // Поиск образца в тексте
        //
        public static int Find(string text, string sample, int startIndex)
        {
            return text.IndexOf(sample, startIndex);
        }
    }
}

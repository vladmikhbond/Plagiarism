using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism
{

    public class XComparer
    {
        public int N { set; get; }

        // It is analized.
        public string  Manuscript { set; get; }

        public XComparer(string m, int n)
        {
            Manuscript = m;
            N = n;
        }

        public XComparer() : this("", 100) { }


        public List<ReportItem> Compare(string bookName, string book)
        {
            var result = new List<ReportItem>();
            int pos = 0;
            // отбираем пробы в манускрипте с шагом N и длиной N/2
            while (pos < Manuscript.Length - N)
            {
                // ищем расширенную пробу в книге 
                var item = IndexOfMax(book, pos);
                
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
            // если проба нашлась, расширяем ее до максимума
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
        public int ExtStart(string book, int ib, int im)
        {
            while (im >= 0 && ib >= 0 && book[ib] == Manuscript[im])
            {
                ib--;
                im--;
            };
            return im + 1;
        }

        // Сдвигаем конец 
        //
        public int ExtFinish(string book, int jb, int jm)
        {
            while (jb < book.Length && jm < Manuscript.Length && book[jb] == Manuscript[jm])
            {
                jb++;
                jm++;
            }
            return jm;
        }


        // Поиск образца в тексте
        //
        public static int Find(string text, string sample, int startIndex)
        {
            return text.IndexOf(sample, startIndex);
        }
    }
}

using System.Collections.Generic;

namespace Plagiarism
{
    /// <summary>
    /// Compare a manuscript with books from library. 
    /// Returns List<ReporItem> as result.
    /// N - step of searching.
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

        // Compare the manuscript with a book
        // 
        public List<ReportItem> Compare(string bookText)
        {
            var result = new List<ReportItem>();
            int pos = 0;
            // отбираем пробы в манускрипте с шагом N 
            while (pos < Manuscript.Length - N)
            {
                // ищем расширенную пробу в книге 
                var item = IndexOfMax(bookText, pos);
                
                if (item != null)
                {
                    result.Add(item);
                    pos += item.Length; 
                }
                pos += N;
            }
            return result;
        }

        // Ищет вхождения части манускрипта в книгу. Часть начинается с позиции pos и имеет длину N символов.
        // Находит только первое вхождение, хотя должен искать максимальное из всех.
        //
        public ReportItem IndexOfMax(string book, int manPos) {
            string manSample = Manuscript.Substring(manPos, N);
            int bookPos = Find (book, manSample, 0);

            // если проба нашлась, расширяем ее до максимального размера
            if (bookPos > -1)
            {
                int im = ExtStart(book, bookPos, manPos);
                int jm = ExtFinish(book, bookPos + N, manPos + N);
                int ib = im - manPos + bookPos; 
                return new ReportItem { ManStart = im, BookStart = ib, Length = jm - im };
            }
            return null;
        }

        // Сдвигаем начало до предела
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

        // Сдвигаем конец до предела
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


        // Поиск образца в тексте (todo: update to KMP later)
        //
        public static int Find(string text, string sample, int startIndex)
        {
            return text.IndexOf(sample, startIndex);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism
{
    public class ReportItem
    {
        public int ManStart;
        public int BookStart;
        public int Length;
        public Book Book;

        public override string ToString()
        {
            return $"{Book.Name}, ({ManStart}, {Length})";
        }

        public string Fragment {
            get { return Book.Slice(BookStart, Length); }                               
        }
    }
}

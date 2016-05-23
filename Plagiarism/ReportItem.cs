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
        public Book Manual;

        public override string ToString()
        {
            return $"{Book.Name}, ({BookStart}, {Length})";
        }

        public string GetBookFragment()
        {          
            return Book.Slice(BookStart, Length); 
        }

        public string GetManualFragment()
        {
            return Manual.Slice(ManStart, Length); 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism
{
    public class ReportItem
    {
        public string BookName;
        public int Start;
        public int Length;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} ", BookName, Start, Length);
        }
    }
}

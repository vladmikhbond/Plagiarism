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
            return $"TAKEN:{Length} from --{BookName} ({Start})";
        }
    }
}

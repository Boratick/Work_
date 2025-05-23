using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminologyApp.Models
{
    public class Term
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Category { get; set; }
        public List<string> References { get; set; }

        public Term(string name, string definition, string category, List<string> references = null)
        {
            Name = name;
            Definition = definition;
            Category = category;
            References = references ?? new List<string>();
        }
    }
}
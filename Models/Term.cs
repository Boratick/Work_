using System;
using System.Collections.Generic;

namespace TerminologyApp.Models
{
    public class Term
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Category { get; set; }
        public List<string> RelatedTerms { get; set; } = new List<string>();
    }
}
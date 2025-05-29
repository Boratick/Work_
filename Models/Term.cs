using System;
using System.Collections.Generic;

namespace TerminologyApp.Models
{
    // Представляє термін у базі даних термінології, включаючи його назву, визначення, категорію та пов’язані терміни.
    public class Term
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public string Category { get; set; }
        public List<string> RelatedTerms { get; set; } = new List<string>();
    }
}
